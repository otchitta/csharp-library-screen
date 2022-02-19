using System;
using System.Collections.ObjectModel;
using Otchitta.Libraries.Screen.Data;
using Otchitta.Libraries.Screen.Data.Dialog;
using Otchitta.Libraries.Screen.Hook;

namespace Otchitta.Demo.Screen.Screen.Dialog;

/// <summary>
/// 選択画面情報クラスです。
/// </summary>
internal sealed class StorageScreenData : AbstractScreenData, OperateScreenData {
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
	/// 基本情報
	/// </summary>
	private string? sourceData;
	/// <summary>
	/// 制限一覧
	/// </summary>
	private ReadOnlyCollection<string>? filterList;
	/// <summary>
	/// 制限情報
	/// </summary>
	private string? filterText;
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
	/// 基本情報を取得または設定します。
	/// </summary>
	/// <value>基本情報</value>
	public string? SourceData {
		get => this.sourceData;
		set => Update(ref this.sourceData, value, nameof(SourceData));
	}
	/// <summary>
	/// 制限一覧を取得します。
	/// </summary>
	/// <returns>制限一覧</returns>
	public ReadOnlyCollection<string> FilterList => this.filterList ??= CreateFilterList();
	/// <summary>
	/// 制限情報を取得または設定します。
	/// </summary>
	/// <value>制限情報</value>
	public string? FilterText {
		get => this.filterText;
		set => Update(ref this.filterText, value, nameof(FilterText));
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
	/// 選択画面情報を生成します。
	/// </summary>
	public StorageScreenData() {
		this.headerText = "ファイルを開く";
		this.detailText = "開くファイルを選択してください。";
		this.sourceData = String.Empty;
		this.filterList = null;
		this.filterText = String.Empty;
		this.remarkText = null;
		this.invokeMenu = null;
		this.listenList = null;
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 制限一覧を生成します。
	/// </summary>
	/// <returns>制限一覧</returns>
	private static ReadOnlyCollection<string> CreateFilterList() {
		var result = new string[] {
			"実行ファイル|*.exe|すべてのファイル|*.*",
			"画像ファイル|*.png;*.jpg;*.bmp;*.gif|すべてのファイル|*.*",
		};
		return new ReadOnlyCollection<string>(result);
	}
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
			var result = new StorageDialogData(this.headerText, this.detailText, this.sourceData ?? String.Empty);
			try {
				result.FilterText = this.filterText ?? String.Empty;
			} catch {
				RemarkText = "制限情報の形式が正しくありません。";
				return;
			}
			this.listenList?.Invoke(this, result);
		}
	}
	#endregion 内部メソッド定義
}
