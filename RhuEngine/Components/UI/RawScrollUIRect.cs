﻿using RhuEngine.WorldObjects;
using RhuEngine.WorldObjects.ECS;

using RNumerics;
using RhuEngine.Linker;
using System.Collections.Generic;

namespace RhuEngine.Components
{
	[UpdateLevel(UpdateEnum.Normal)]
	[Category(new string[] { "UI" })]
	public abstract class RawScrollUIRect : UIRect
	{
		[OnChanged(nameof(ScrollPosChange))]	
		public Sync<Vector2f> ScrollPos;

		public virtual Vector2f MaxScroll => Vector2f.Inf;


		public virtual Vector2f MinScroll => Vector2f.NInf;

		[Default(true)]
		public Sync<bool> LaserScroll;

		[Default(true)]
		public Sync<bool> TouchScroll;

		internal void ScrollPosChange() {
			_childRects.SafeOperation((list) => {
				foreach (var item in list) {
					item.Scroll(ScrollPos.Value.XY_ + ParentRect.ScrollOffset);
				}
			});
		}

		public override void OnLoaded() {
			base.OnLoaded();
			ScrollPosChange();
		}

		public override void Step() {
			if (LaserScroll) {
				//Todo: if hover do this
				var scroll = RInput.Mouse.ScrollChange;
				if (!(scroll.x == 0 && scroll.y == 0)) {
					var newvalue = ScrollPos.Value + scroll;
					ScrollPos.Value = MathUtil.Clamp(newvalue, MinScroll, MaxScroll);
				}
				
			}
		}
	}
}
