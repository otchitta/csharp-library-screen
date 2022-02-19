using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Otchitta.Libraries.Screen.Data.Dialog;

/// <summary>
/// 選択分類情報クラスです。
/// </summary>
public sealed class StorageDivideData : AbstractScreenData {
	#region メンバー変数定義
	/// <summary>
	/// 要素情報
	/// </summary>
	private readonly string sourceData;
	/// <summary>
	/// 要素名称
	/// </summary>
	private readonly string sourceName;
	/// <summary>
	/// 抽出処理
	/// </summary>
	private readonly Func<IEnumerable<StorageDivideData>> chooseHook;
	/// <summary>
	/// 展開状態
	/// </summary>
	private bool expandFlag;
	/// <summary>
	/// 選択状態
	/// </summary>
	private bool selectFlag;
	/// <summary>
	/// 下位一覧
	/// </summary>
	private ReadOnlyCollection<StorageDivideData>? clientList;
	/// <summary>
	/// 通知一覧
	/// </summary>
	private EventHandler? selectHook;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <value>要素情報</value>
	public string SourceData => this.sourceData;
	/// <summary>
	/// 要素名称を取得します。
	/// </summary>
	/// <value>要素名称</value>
	public string SourceName => this.sourceName;
	/// <summary>
	/// 展開状態を取得または設定します。
	/// </summary>
	/// <value>展開状態</value>
	public bool ExpandFlag {
		get => this.expandFlag;
		set => Update(ref this.expandFlag, value, nameof(ExpandFlag));
	}
	/// <summary>
	/// 選択状態を取得または設定します。
	/// </summary>
	/// <value>選択状態</value>
	public bool SelectFlag {
		get => this.selectFlag;
		set => Update(ref this.selectFlag, value, nameof(SelectFlag), ActionSelectFlag);
	}
	/// <summary>
	/// 下位一覧を取得します。
	/// </summary>
	/// <returns>下位一覧</returns>
	public ReadOnlyCollection<StorageDivideData> ClientList => this.clientList ??= CreateClientList();
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
	/// 分類情報を生成します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="sourceName">要素名称</param>
	/// <param name="chooseHook">抽出処理</param>
	public StorageDivideData(string sourceData, string sourceName, Func<IEnumerable<StorageDivideData>> chooseHook) {
		this.sourceData = sourceData;
		this.sourceName = sourceName;
		this.chooseHook = chooseHook;
		this.expandFlag = false;
		this.selectFlag = false;
		this.clientList = null;
		this.selectHook = null;
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 下位情報を処理します。
	/// </summary>
	/// <param name="source">発信情報</param>
	/// <param name="option">引数情報</param>
	private void ActionClientData(object? source, EventArgs option) =>
		this.selectHook?.Invoke(this, EventArgs.Empty);
	/// <summary>
	/// 下位一覧を生成します。
	/// </summary>
	/// <returns>下位一覧</returns>
	private ReadOnlyCollection<StorageDivideData> CreateClientList() {
		var result = new List<StorageDivideData>();
		try {
			foreach (var choose in this.chooseHook()) {
				result.Add(choose);
			}
		} catch {
			// 処理なし
		}
		return new ReadOnlyCollection<StorageDivideData>(result);
	}
	/// <summary>
	/// 選択状態を処理します。
	/// </summary>
	private void ActionSelectFlag() => this.selectHook?.Invoke(this, EventArgs.Empty);
	#endregion 内部メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 選択情報を抽出します。
	/// </summary>
	/// <param name="result">選択情報</param>
	/// <returns>選択情報が存在する場合、<c>True</c>を返却</returns>
	public bool ChooseSelectData([MaybeNullWhen(false)]out StorageDivideData result) {
		if (this.selectFlag) {
			// 選択状態である場合
			result = this;
			return true;
		} else if (this.clientList == null) {
			// 下位一覧がない場合
			result = default;
			return false;
		} else {
			// 下位判定を行う場合
			foreach (var clientData in this.clientList) {
				if (clientData.ChooseSelectData(out result)) {
					return true;
				}
			}
			result = default;
			return false;
		}
	}
	#endregion 公開メソッド定義
}
