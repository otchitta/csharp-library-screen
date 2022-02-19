using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Otchitta.Demo.Screen.Screen.Dialog;
using Otchitta.Libraries.Screen.Data;
using Otchitta.Libraries.Screen.Data.Dialog;

namespace Otchitta.Demo.Screen.Screen;

/// <summary>
/// 主要画面情報クラスです。
/// </summary>
internal sealed class MainScreenData : AbstractScreenData {
	#region メンバー変数定義
	/// <summary>
	/// 選択一覧
	/// </summary>
	private ReadOnlyCollection<Tuple<string, OperateScreenData>>? selectList;
	/// <summary>
	/// 選択情報
	/// </summary>
	private object? selectData;
	/// <summary>
	/// 通知情報
	/// </summary>
	private object? dialogData;
	/// <summary>
	/// 状態内容
	/// </summary>
	private string? statusText;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 選択一覧を取得します。
	/// </summary>
	/// <value>選択一覧</value>
	public ReadOnlyCollection<Tuple<string, OperateScreenData>> SelectList => this.selectList ??= CreateSelectList();
	/// <summary>
	/// 選択情報を取得します。
	/// </summary>
	/// <value>選択情報</value>
	public object? SelectData {
		get => this.selectData;
		set => Update(ref this.selectData, value, nameof(SelectData));
	}
	/// <summary>
	/// 画面情報を取得します。
	/// </summary>
	/// <value>画面情報</value>
	public object? DialogData {
		get => this.dialogData;
		private set => Update(ref this.dialogData, value, nameof(DialogData));
	}
	/// <summary>
	/// 状態内容を取得します。
	/// </summary>
	/// <value>状態内容</value>
	public string? StatusText {
		get => this.statusText;
		private set => Update(ref this.statusText, value, nameof(StatusText));
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 主要画面情報を生成します。
	/// </summary>
	public MainScreenData() {
		this.selectList = null;
		this.selectData = null;
		this.dialogData = null;
		this.statusText = null;
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 状態内容を生成します。
	/// </summary>
	/// <param name="source">選択情報</param>
	/// <returns>状態内容</returns>
	private string ChooseStatusText(bool? source) {
		switch (source) {
			default:    return "選択情報：不明";
			case true:  return "選択情報：許可";
			case false: return "選択情報：取消";
		}
	}
	/// <summary>
	/// 画面情報を処理します。
	/// </summary>
	/// <param name="source">発信情報</param>
	/// <param name="option">引数情報</param>
	private void ActionDialogData(object? source, EventArgs option) {
		if (source is MessageDialogData cache1) {
			cache1.SelectHook -= ActionDialogData;
		} else if (source is ConfirmDialogData cache2) {
			cache2.SelectHook -= ActionDialogData;
			StatusText = ChooseStatusText(cache2.SelectData);
		} else if (source is WarningDialogData cache3) {
			cache3.SelectHook -= ActionDialogData;
			StatusText = ChooseStatusText(cache3.SelectData);
		} else if (source is StorageDialogData cache4) {
			cache4.SelectHook -= ActionDialogData;
			StatusText = $"選択情報：{cache4.SelectFile}";
		}
		DialogData = null;
	}
	/// <summary>
	/// 選択情報を処理します。
	/// </summary>
	/// <param name="source">発信情報</param>
	/// <param name="option">引数情報</param>
	private void ActionSelectData(object? source, object option) {
		if (option is MessageDialogData cache1) {
			cache1.SelectHook += ActionDialogData;
		} else if (option is ConfirmDialogData cache2) {
			cache2.SelectHook += ActionDialogData;
		} else if (option is WarningDialogData cache3) {
			cache3.SelectHook += ActionDialogData;
		} else if (option is StorageDialogData cache4) {
			cache4.SelectHook += ActionDialogData;
		}
		DialogData = option;
	}
	/// <summary>
	/// 選択集合を生成します。
	/// </summary>
	/// <returns>選択集合</returns>
	private static IEnumerable<(string, OperateScreenData)> CreateSourceList() {
		yield return ("通知ダイアログ", new MessageScreenData());
		yield return ("確認ダイアログ", new ConfirmScreenData());
		yield return ("警告ダイアログ", new WarningScreenData());
		yield return ("障害ダイアログ", new FailureScreenData());
		yield return ("選択ダイアログ", new StorageScreenData());
	}
	/// <summary>
	/// 選択一覧を生成します。
	/// </summary>
	/// <returns>選択一覧</returns>
	private ReadOnlyCollection<Tuple<string, OperateScreenData>> CreateSelectList() {
		var result = new List<Tuple<string, OperateScreenData>>();
		foreach (var (value1, value2) in CreateSourceList()) {
			value2.InvokeHook += ActionSelectData;
			result.Add(Tuple.Create(value1, value2));
		}
		return new ReadOnlyCollection<Tuple<string, OperateScreenData>>(result);
	}
	#endregion 内部メソッド定義
}
