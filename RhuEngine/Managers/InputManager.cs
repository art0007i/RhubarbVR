﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RhuEngine.AssetSystem;
using RhuEngine.AssetSystem.AssetProtocals;
using System.IO;
using System.Threading.Tasks;
using StereoKit;
using HidSharp;
using HidSharp.Utility;
using RhuEngine.Settings;

namespace RhuEngine.Managers
{
	public class InputManager : IManager
	{
		private Engine _engine;

		public enum InputTypes
		{
			None,
			StickLocker,
			MoveSpeed,
			Back,
			Forward,
			Left,
			Right,
			Jump,
			FlyUp,
			FlyDown,
			RotateLeft,
			RotateRight,
			ObjectPull,
			ObjectPush,
			OpenDash,
			ChangeWorld,
			ContextMenu,
			Primary,
			Secondary,
			Grab,
		}


		public void Dispose() {
		}

		public void Init(Engine engine) {
			_engine = engine;
			foreach (var item in DeviceList.Local.GetHidDevices()) {
				Log.Info($"{item.GetFriendlyName()}");
			}
		}

		public float GetInputFloatFromKeyboard(InputTypes inputType) {
			var keyboard = _engine.MainSettings.InputSettings.KeyboardInputSettings;
			var keyboardInput = inputType switch {
				InputTypes.MoveSpeed => keyboard.Sprint.GetInput() ? 1.0f : keyboard.SlowCraw.GetInput() ? 0f : 0.3f,
				InputTypes.Back => keyboard.Back.GetInput() ? 1.0f : 0.0f,
				InputTypes.Forward => keyboard.Forward.GetInput() ? 1.0f : 0.0f,
				InputTypes.Left => keyboard.Left.GetInput() ? 1.0f : 0.0f,
				InputTypes.Right => keyboard.Right.GetInput() ? 1.0f : 0.0f,
				InputTypes.Jump => keyboard.Jump.GetInput() ? 1.0f : 0.0f,
				InputTypes.FlyUp => keyboard.FlyUP.GetInput() ? 1.0f : 0.0f,
				InputTypes.FlyDown => keyboard.FlyDown.GetInput() ? 1.0f : 0.0f,
				InputTypes.RotateLeft => keyboard.RotateLeft.GetInput() ? 1.0f : 0.0f,
				InputTypes.RotateRight => keyboard.RotateRight.GetInput() ? 1.0f : 0.0f,
				InputTypes.OpenDash => keyboard.Dash.GetInput() ? 1.0f : 0.0f,
				InputTypes.ChangeWorld => keyboard.SwitchWorld.GetInput() ? 1.0f : 0.0f,
				InputTypes.ContextMenu => keyboard.ContextMenu.GetInput() ? 1.0f : 0.0f,
				InputTypes.Primary => keyboard.PrimaryPress.GetInput() ? 1.0f : 0.0f,
				InputTypes.Secondary => keyboard.SecondaryPress.GetInput() ? 1.0f : 0.0f,
				InputTypes.Grab => keyboard.Grab.GetInput() ? 1.0f : 0.0f,
				InputTypes.ObjectPull => keyboard.ObjectPull.GetInput() ? 1.0f : 0.0f,
				InputTypes.ObjectPush => keyboard.ObjectPush.GetInput() ? 1.0f : 0.0f,
				_ => 0,
			};
			if (keyboard.MousePositive == inputType) {
				keyboardInput = Input.Mouse.scrollChange;
			}
			if (keyboard.MouseNegevitve == inputType) {
				keyboardInput = -Input.Mouse.scrollChange;
			}
			return keyboardInput;
		}
		public float GetInputFloatFromGamePad(InputTypes inputType) {
			switch (inputType) {
				case InputTypes.None:
					break;
				case InputTypes.StickLocker:
					break;
				case InputTypes.MoveSpeed:
					break;
				case InputTypes.Back:
					break;
				case InputTypes.Forward:
					break;
				case InputTypes.Left:
					break;
				case InputTypes.Right:
					break;
				case InputTypes.Jump:
					break;
				case InputTypes.FlyUp:
					break;
				case InputTypes.FlyDown:
					break;
				case InputTypes.RotateLeft:
					break;
				case InputTypes.RotateRight:
					break;
				case InputTypes.ObjectPull:
					break;
				case InputTypes.ObjectPush:
					break;
				case InputTypes.OpenDash:
					break;
				case InputTypes.ChangeWorld:
					break;
				case InputTypes.ContextMenu:
					break;
				case InputTypes.Primary:
					break;
				case InputTypes.Secondary:
					break;
				case InputTypes.Grab:
					break;
				default:
					break;
			}
			return 0f;
		}

		public float GetInputFloatFromController(InputTypes inputType,Controller controller, ControllerInputSettingsObject controllerInput) {
			return controllerInput.GetInputFloatFromController(inputType,controller);
		}

		public float GetInputFloatFromMainController(InputTypes inputType) {
			return GetInputFloatFromController(inputType, _engine.MainSettings.InputSettings.RightHanded ? Input.Controller(Handed.Right) : Input.Controller(Handed.Left), _engine.MainSettings.InputSettings.MainControllerInputSettings);
		}
		public float GetInputFloatFromSeccondController(InputTypes inputType) {
			return GetInputFloatFromController(inputType, _engine.MainSettings.InputSettings.RightHanded ? Input.Controller(Handed.Left) : Input.Controller(Handed.Right), _engine.MainSettings.InputSettings.MainControllerInputSettings);
		}
		public float GetInputFloat(InputTypes inputType,bool? mainController = null) {
			var main = GetInputFloatFromKeyboard(inputType) + GetInputFloatFromGamePad(inputType);
			if(mainController is null) {
				main += GetInputFloatFromMainController(inputType);
				main += GetInputFloatFromSeccondController(inputType);
			}
			else {
				if (mainController.Value) {
					main += GetInputFloatFromMainController(inputType);
				}
				else {
					main += GetInputFloatFromSeccondController(inputType);
				}
			}
			return Math.Max(Math.Min(main, 0), 1);
		}

		public bool GetInputBool(InputTypes inputType, bool? mainController = null) {
			return GetInputFloat(inputType, mainController) >= 0.9f;
		}

		public void Step() {
		}
	}
}
