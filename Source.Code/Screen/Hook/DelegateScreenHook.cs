using System;

namespace Otchitta.Libraries.Screen.Hook;

/// <summary>
/// <see cref="Delegate" />利用画面操作クラスです。
/// </summary>
public sealed class DelegateScreenHook : AbstractScreenHook {
	/// <summary>
	/// 判定処理
	/// </summary>
	private Predicate<object?> accept;
	/// <summary>
	/// 実行処理
	/// </summary>
	private Action<object?> invoke;

	/// <summary>
	/// <see cref="Delegate" />利用画面操作を生成します。
	/// </summary>
	/// <param name="invoke">実行処理</param>
	/// <param name="accept">判定処理</param>
	public DelegateScreenHook(Action<object?> invoke, Predicate<object?> accept) {
		this.invoke = invoke;
		this.accept = accept;
	}
	/// <summary>
	/// <see cref="Delegate" />利用画面操作を生成します。
	/// </summary>
	/// <param name="invoke">実行処理</param>
	public DelegateScreenHook(Action<object?> invoke) : this(invoke, parameter => true) {
		// 処理なし
	}

	/// <summary>
	/// <see cref="Delegate" />利用画面操作を生成します。
	/// </summary>
	/// <param name="invoke">実行処理</param>
	/// <param name="accept">判定処理</param>
	public DelegateScreenHook(Action invoke, Func<bool> accept) {
		this.accept = parameter => accept();
		this.invoke = parameter => invoke();
	}
	/// <summary>
	/// <see cref="Delegate" />利用画面操作を生成します。
	/// </summary>
	/// <param name="invoke">実行処理</param>
	public DelegateScreenHook(Action invoke) : this(invoke, () => true) {
		// 処理なし
	}

	/// <summary>
	/// 状態変更を通知します。
	/// </summary>
	public new void Notify() => base.Notify();

	/// <summary>
	/// 操作可否を判定します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	/// <returns>操作可能である場合、<c>True</c>を返却</returns>
	protected override bool Accept(object? parameter) => this.accept(parameter);
	/// <summary>
	/// 操作処理を実行します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	protected override void Invoke(object? parameter) => this.invoke(parameter);
}
