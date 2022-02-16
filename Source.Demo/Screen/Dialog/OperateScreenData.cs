using System;

namespace Otchitta.Demo.Screen.Screen.Dialog;

/// <summary>
/// 操作画面情報インターフェースです。
/// </summary>
internal interface OperateScreenData {
	/// <summary>
	/// 実行処理を追加または削除します。
	/// </summary>
	event EventHandler<object>? InvokeHook;
}
