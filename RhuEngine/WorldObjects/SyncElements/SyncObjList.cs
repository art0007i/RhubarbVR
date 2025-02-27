﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using RhuEngine.DataStructure;
using RhuEngine.Datatypes;

namespace RhuEngine.WorldObjects
{
	public interface ISyncObjectList<T> : ISyncObject
	{
		public T Add(bool networkedObject = false, bool deserialize = false);
	}
	public class SyncObjList<T> : SyncListBase<T>, ISyncObjectList<T>, INetworkedObject, IEnumerable<ISyncObject> where T : ISyncObject, new()
	{
		public T AddWithCustomRefIds(bool networkedObject = false, bool deserialize = false,Func<NetPointer> func = null) {
			var newElement = new T();
			newElement.Initialize(World, this, "List Elemenet", networkedObject, deserialize, func);
			AddInternal(newElement);
			return newElement;
		}

		public T Add(bool networkedObject = false, bool deserialize = false) {
			var newElement = new T();
			newElement.Initialize(World, this, "List Elemenet", networkedObject, deserialize);
			if (!networkedObject) {
				BroadcastAdd(newElement);
			}
			AddInternal(newElement);
			return newElement;
		}

		public override void InitializeMembers(bool networkedObject, bool deserialize, Func<NetPointer> func) {
		}
		public override IDataNode Serialize(SyncObjectSerializerObject syncObjectSerializerObject) {
			return syncObjectSerializerObject.CommonListSerialize(this, this);
		}

		public override void Deserialize(IDataNode data, SyncObjectDeserializerObject syncObjectSerializerObject) {
			syncObjectSerializerObject.ListDeserialize((DataNodeGroup)data, this);
		}

		public override T LoadElement(IDataNode data) {
			var newElement = new T();
			newElement.Initialize(World, this, "List Elemenet", true, false);
			newElement.Deserialize(data, new SyncObjectDeserializerObject(false));
			return newElement;
		}

		public override IDataNode SaveElement(T val) {
			return val.Serialize(new SyncObjectSerializerObject(true));
		}
	}
}
