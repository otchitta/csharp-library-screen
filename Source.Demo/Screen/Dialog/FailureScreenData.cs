using System;
using Otchitta.Libraries.Screen.Data;
using Otchitta.Libraries.Screen.Data.Dialog;
using Otchitta.Libraries.Screen.Hook;

namespace Otchitta.Demo.Screen.Screen.Dialog;

/// <summary>
/// 確認画面情報クラスです。
/// </summary>
internal sealed class FailureScreenData : AbstractScreenData, OperateScreenData {
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
	public FailureScreenData() {
		this.headerText = "処理障害";
		this.detailText = "緊急メンテナンス中により全ての操作が行えなくなりました。\r\n"
		                + "NN時間後までは当該システムは利用出来ません。\r\n"
		                + "※ご連絡なくメンテナンスが延長される可能性がありますので、お手数ですが、\r\r"
		                + "　当該アプリを再起動後、今一度下記「メンテナンス終了日時」をご確認ください\r\n"
		                + "\r\n"
		                + "メンテナンス終了日時：YYYY-MM-DD HH:MM\r\n"
		                + "※発注ミスなどによる緊急事態には、下記、「管理部門」へのご連絡をお願いいたします\r\n"
		                + "管理部門：00-0000-0000\r\n"
		                + "\r\n"
		                + "上記「メンテナンス終了日時」を超えて当該メッセージが表示される場合、誠にお手数ですが下記連絡先にご連絡をお願いいたします\r\r"
		                + "緊急連絡先：000-0000-0000";
		this.remarkText = "「実行」操作にて、デモアプリは再起動までは操作が不可となります。";
		this.invokeMenu = null;
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
			this.listenList?.Invoke(this, new FailureDialogData(this.headerText, this.detailText));
		}
	}
	#endregion 内部メソッド定義
}
