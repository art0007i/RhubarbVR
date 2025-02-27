﻿using System;

using RhuEngine.DataStructure;
using RhuEngine.Datatypes;

namespace RhuEngine.WorldObjects
{
	public class SyncValueStream<T> : SyncStream, ILinkerMember<T>, ISync, INetworkedObject, IChangeable
	{
		private readonly object _locker = new();

		private T _value;

		public T Value
		{
			get => _value;
			set {
				lock (_locker) {
					_value = value;
					BroadcastValue();
					Changed?.Invoke(this);
				}
			}
		}

		private void BroadcastValue() {
			if (IsLinked || NoSync) {
				return;
			}
			World.BroadcastDataToAll(this, typeof(T).IsEnum ? new DataNode<int>((int)(object)_value) : new DataNode<T>(_value), LiteNetLib.DeliveryMethod.Unreliable);
		}
		public override void Received(Peer sender, IDataNode data) {
			if (NoSync) {
				return;
			}
			var newValue = typeof(T).IsEnum ? (T)(object)((DataNode<int>)data).Value : ((DataNode<T>)data).Value;
			lock (_locker) {
				_value = newValue;
				Changed?.Invoke(this);
			}
		}

		public event Action<IChangeable> Changed;

		public void SetValue(object value) {
			lock (_locker) {
				try {
					Value = (T)value;
				}
				catch { }
			}
		}

		public void SetValueNoOnChange(T value) {
			_value = value;
			BroadcastValue();
		}

		public void SetValueNoOnChangeAndNetworking(T value) {
			_value = value;
		}

		public bool IsLinked { get; private set; }

		[NoSave]
		[NoSync]
		[NoShow]
		public ILinker drivenFromObj;

		public NetPointer LinkedFrom => drivenFromObj.Pointer;

		public void KillLink() {
			drivenFromObj.RemoveLinkLocation();
			IsLinked = false;
		}

		public void Link(ILinker value) {
			if (!IsLinked) {
				ForceLink(value);
			}
		}
		public void ForceLink(ILinker value) {
			if (IsLinked) {
				KillLink();
			}
			value.SetLinkLocation(this);
			drivenFromObj = value;
			IsLinked = true;
		}

	}
}
