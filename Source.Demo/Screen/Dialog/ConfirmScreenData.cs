using System;
using Otchitta.Libraries.Screen.Data;
using Otchitta.Libraries.Screen.Data.Dialog;
using Otchitta.Libraries.Screen.Hook;

namespace Otchitta.Demo.Screen.Screen.Dialog;

/// <summary>
/// 確認画面情報クラスです。
/// </summary>
public sealed class ConfirmScreenData : AbstractScreenData, OperateScreenData {
	#region メンバー変数定義
	/// <summary>
	/// 表題内容
	/// </summary>
	private string? headerText;
	/// <summary>
	/// 詳細内容
	/// </summary>
	private string? detailText;
	/// <summary>
	/// 備考内容
	/// </summary>
	private string? remarkText;
	/// <summary>
	/// 実行操作
	/// </summary>
	private AbstractScreenHook? invokeMenu;
	/// <summary>
	/// 通知一覧
	/// </summary>
	private event EventHandler<object>? listenList;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 表題内容を取得または設定します。
	/// </summary>
	/// <value>表題内容</value>
	public string? HeaderText {
		get => this.headerText;
		set => Update(ref this.headerText, value, nameof(HeaderText));
	}
	/// <summary>
	/// 詳細内容を取得または設定します。
	/// </summary>
	/// <value>詳細内容</value>
	public string? DetailText {
		get => this.detailText;
		set => Update(ref this.detailText, value, nameof(DetailText));
	}
	/// <summary>
	/// 備考内容を取得します。
	/// </summary>
	/// <value>備考内容</value>
	public string? RemarkText {
		get => this.remarkText;
		private set => Update(ref this.remarkText, value, nameof(RemarkText));
	}
	/// <summary>
	/// 実行操作を取得します。
	/// </summary>
	/// <value>実行操作</value>
	public AbstractScreenHook InvokeMenu => this.invokeMenu ??= new DelegateScreenHook(ActionInvokeMenu);
	#endregion プロパティー定義

	#region 公開イベント定義
	/// <summary>
	/// 実行処理を追加または削除します。
	/// </summary>
	event EventHandler<object>? OperateScreenData.InvokeHook {
		add    => this.listenList += value;
		remove => this.listenList -= value;
	}
	#endregion 公開イベント定義

	#region 生成メソッド定義
	/// <summary>
	/// 確認画面情報を生成します。
	/// </summary>
	public ConfirmScreenData() {
		this.headerText = "登録確認";
		this.detailText = "以下のデータを登録しますがよろしいですか？\r\n番号：ITEM0001\r\n名称：デモプログラム\r\n備考：画面デモを行うクラス";
		this.remarkText = null;
		this.invokeMenu = null;
		this.listenList = null;
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 選択操作を処理します。
	/// </summary>
	private void ActionInvokeMenu() {
		if (String.IsNullOrEmpty(this.headerText)) {
			RemarkText = "表題内容を入力してください。";
		} else if (String.IsNullOrEmpty(this.detailText)) {
			RemarkText = "詳細内容を入力してください。";
		} else {
			RemarkText = null;
			this.listenList?.Invoke(this, new ConfirmDialogData(this.headerText, this.detailText));
		}
	}
	#endregion 内部メソッド定義
}
