namespace Otchitta.Libraries.Screen.Data.Dialog;

/// <summary>
/// 警告画面情報クラスです。
/// </summary>
public sealed class FailureDialogData {
	#region メンバー変数定義
	/// <summary>
	/// 表題内容
	/// </summary>
	private readonly string headerText;
	/// <summary>
	/// 詳細情報
	/// </summary>
	private readonly object detailData;
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
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 確認画面情報を生成します。
	/// </summary>
	/// <param name="headerText">表題内容</param>
	/// <param name="detailData">詳細情報</param>
	public FailureDialogData(string headerText, object detailData) {
		this.headerText = headerText;
		this.detailData = detailData;
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
	#endregion 内部メソッド定義
}
