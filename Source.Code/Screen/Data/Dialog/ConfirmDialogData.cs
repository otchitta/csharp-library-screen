using System;
using Otchitta.Libraries.Screen.Hook;

namespace Otchitta.Libraries.Screen.Data.Dialog;

/// <summary>
/// 確認画面情報クラスです。
/// </summary>
public sealed class ConfirmDialogData : AbstractScreenData {
	#region メンバー変数定義
	/// <summary>
	/// 表題内容
	/// </summary>
	private readonly string headerText;
	/// <summary>
	/// 詳細情報
	/// </summary>
	private readonly object detailData;
	/// <summary>
	/// 選択操作
	/// </summary>
	private AbstractScreenHook? selectMenu;
	/// <summary>
	/// 選択情報
	/// </summary>
	private bool? selectData;
	/// <summary>
	/// 通知一覧
	/// </summary>
	private event EventHandler? selectHook;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 表題内容を取得します。
	/// </summary>
	/// <value>表題内容</value>
	public string HeaderText => this.headerText;
	/// <summary>
	/// 詳細情報を取得します。
	/// </summary>
	/// <value>詳細情報</value>
	public object DetailData => this.detailData;
	/// <summary>
	/// 選択操作を取得します。
	/// </summary>
	/// <value>選択操作</value>
	public AbstractScreenHook SelectMenu => this.selectMenu ??= new DelegateScreenHook(ActionSelectMenu);
	/// <summary>
	/// 選択情報を取得します。
	/// </summary>
	/// <value>選択情報</value>
	public bool? SelectData {
		get => this.selectData;
		private set => Update(ref this.selectData, value, nameof(SelectData));
	}
	#endregion プロパティー定義

	#region 公開イベント定義
	/// <summary>
	/// 選択処理を追加または削除します。
	/// </summary>
	public event EventHandler? SelectHook {
		add    => this.selectHook += value;
		remove => this.selectHook -= value;
	}
	#endregion 公開イベント定義

	#region 生成メソッド定義
	/// <summary>
	/// 確認画面情報を生成します。
	/// </summary>
	/// <param name="headerText">表題内容</param>
	/// <param name="detailData">詳細情報</param>
	public ConfirmDialogData(string headerText, object detailData) {
		this.headerText = headerText;
		this.detailData = detailData;
		this.selectMenu = null;
		this.selectData = null;
		this.selectHook = null;
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 選択情報を選出します。
	/// </summary>
	/// <param name="source">操作情報</param>
	/// <returns>選択情報</returns>
	private static bool? ChooseSelectData(string? source) {
		switch(source?.ToLowerInvariant()) {
			default:       return null;
			case "accept": return true;
			case "cancel": return false;
		}
	}
	/// <summary>
	/// 選択操作を処理します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	private void ActionSelectMenu(object? parameter) {
		SelectData = ChooseSelectData(parameter?.ToString());
		this.selectHook?.Invoke(this, EventArgs.Empty);
	}
	#endregion 内部メソッド定義
}
