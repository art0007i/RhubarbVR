﻿using RhuEngine.WorldObjects;
using RhuEngine.WorldObjects.ECS;

using StereoKit;

namespace RhuEngine.Components
{
	[Category(new string[] { "UI\\Visuals" })]
	public class UISameLine : UIComponent
	{
		public override void RenderUI() {
			UI.SameLine();
		}
	}
}
