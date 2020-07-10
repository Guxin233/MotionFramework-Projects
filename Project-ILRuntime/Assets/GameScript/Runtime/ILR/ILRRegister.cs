﻿using System;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;

public static class ILRRegister
{
	public static void Register(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
	{
		// 注册委托
		appdomain.DelegateManager.RegisterMethodDelegate<float>();
		appdomain.DelegateManager.RegisterMethodDelegate<bool>();
		appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
		appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
		appdomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Network.INetworkPackage>();
		appdomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Event.IEventMessage>();
		appdomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Resource.AssetOperationHandle>();
		appdomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Resource.SceneInstance>();
		appdomain.DelegateManager.RegisterMethodDelegate<MotionFramework.Config.AssetConfig>();
		appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();

		// 注册补间系统委托
		appdomain.DelegateManager.RegisterFunctionDelegate<float, float, float, float, float>();
		appdomain.DelegateManager.RegisterFunctionDelegate<float, float, float, float> ();
		appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector2, UnityEngine.Vector2, float, UnityEngine.Vector2>();
		appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector3, UnityEngine.Vector3, float, UnityEngine.Vector3>();
		appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector4, UnityEngine.Vector4, float, UnityEngine.Vector4>();
		appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Color, UnityEngine.Color, float, UnityEngine.Color>();
		appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Quaternion, UnityEngine.Quaternion, float, UnityEngine.Quaternion>();

		// 注册委托转换器
		appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
		{
			return new UnityEngine.Events.UnityAction(() =>
			{
				((Action)act)();
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<float>>((act) =>
		{
			return new UnityEngine.Events.UnityAction<float>((arg0) =>
			{
				((Action<float>)act)(arg0);
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<bool>>((act) =>
		{
			return new UnityEngine.Events.UnityAction<bool>((arg0) =>
			{
				((Action<bool>)act)(arg0);
			});
		});
		appdomain.DelegateManager.RegisterDelegateConvertor<MotionFramework.Tween.TweenNode<UnityEngine.Vector2>.TweenLerpDelegate>((act) =>
		{
			return new MotionFramework.Tween.TweenNode<UnityEngine.Vector2>.TweenLerpDelegate((from, to, progress) =>
			{
				return ((Func<UnityEngine.Vector2, UnityEngine.Vector2, System.Single, UnityEngine.Vector2>)act)(from, to, progress);
			});
		});

		// 注册值类型绑定器
		appdomain.RegisterValueTypeBinder(typeof(UnityEngine.Vector2), new Vector2Binder());
		appdomain.RegisterValueTypeBinder(typeof(UnityEngine.Vector3), new Vector3Binder());
		appdomain.RegisterValueTypeBinder(typeof(UnityEngine.Quaternion), new QuaternionBinder());

		// 注册适配器
		appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
		appdomain.RegisterCrossBindingAdaptor(new CanvasWindowAdapter());
		appdomain.RegisterCrossBindingAdaptor(new Google.Protobuf.IMessageAdapter());
		appdomain.RegisterCrossBindingAdaptor(new MotionFramework.AI.IFsmNodeAdapter());
		appdomain.RegisterCrossBindingAdaptor(new MotionFramework.Event.IEventMessageAdapter());
		appdomain.RegisterCrossBindingAdaptor(new MotionFramework.Config.AssetConfigAdapter());
		appdomain.RegisterCrossBindingAdaptor(new MotionFramework.Config.ConfigTableAdapter());
		
		// 执行CLR绑定
		//ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);
		Type classCLRBinding = Type.GetType("ILRuntime.Runtime.Generated.CLRBindings");
		if (classCLRBinding != null)
		{
			var method = classCLRBinding.GetMethod("Initialize");
			method.Invoke(null, new object[] { appdomain });
		}
		else
		{
			GameLog.Warning("ILRuntime not generated binding scripts.");

			// 注册LitJson
			LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
		}
	}
}