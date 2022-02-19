using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Otchitta.Libraries.Screen.Hook;

namespace Otchitta.Libraries.Screen.Data.Dialog;

/// <summary>
/// 選択画面情報クラスです。
/// </summary>
public sealed class StorageDialogData : AbstractScreenData {
	#region メンバー変数定義
	/// <summary>
	/// 表題内容
	/// </summary>
	private readonly string headerText;
	/// <summary>
	/// 詳細内容
	/// </summary>
	private readonly string detailText;
	/// <summary>
	/// 基本情報
	/// </summary>
	private readonly string sourceData;
	/// <summary>
	/// 制限情報
	/// </summary>
	private string filterText;
	/// <summary>
	/// 制限一覧
	/// </summary>
	private ObservableCollection<StorageFilterData> filterList;
	/// <summary>
	/// 制限番号
	/// </summary>
	private int filterCode;
	/// <summary>
	/// 分類情報
	/// </summary>
	private StorageDivideData? divideData;
	/// <summary>
	/// 詳細一覧
	/// </summary>
	private ObservableCollection<StorageDetailData> detailList;
	/// <summary>
	/// 選択情報
	/// </summary>
	private string? selectData;
	/// <summary>
	/// 選択情報
	/// </summary>
	private FileInfo? selectFile;
	/// <summary>
	/// 選択操作
	/// </summary>
	private AbstractScreenHook? selectMenu;
	/// <summary>
	/// 状態内容
	/// </summary>
	private string? statusText;
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
	/// 詳細内容を取得します。
	/// </summary>
	/// <value>詳細内容</value>
	public string DetailText => this.detailText;
	/// <summary>
	/// 制限情報を取得または設定します。
	/// </summary>
	/// <value>制限情報</value>
	public string FilterText {
		get => this.filterText;
		set => Update(ref this.filterText, value, nameof(FilterText), ActionFilterText);
	}
	/// <summary>
	/// 制限一覧を取得します。
	/// </summary>
	/// <value>制限一覧</value>
	public ReadOnlyObservableCollection<StorageFilterData> FilterList {
		get;
	}
	/// <summary>
	/// 制限番号を取得または設定します。
	/// </summary>
	/// <value>制限番号</value>
	public int FilterCode {
		get => this.filterCode;
		set => Update(ref this.filterCode, value, nameof(FilterCode), ActionFilterCode);
	}
	/// <summary>
	/// 分類情報を取得します。
	/// </summary>
	/// <value>分類情報</value>
	public StorageDivideData DivideData => this.divideData ??= CreateDivideData();
	/// <summary>
	/// 詳細一覧を取得します。
	/// </summary>
	/// <value>詳細一覧</value>
	public ReadOnlyObservableCollection<StorageDetailData> DetailList {
		get;
	}
	/// <summary>
	/// 選択情報を取得または設定します。
	/// </summary>
	/// <value>選択情報</value>
	public string? SelectData {
		get => this.selectData;
		set => Update(ref this.selectData, value, nameof(SelectData));
	}
	/// <summary>
	/// 選択情報を取得します。
	/// </summary>
	/// <value>選択情報</value>
	public FileInfo? SelectFile {
		get => this.selectFile;
		private set => Update(ref this.selectFile, value, nameof(SelectFile));
	}
	/// <summary>
	/// 選択操作を取得します。
	/// </summary>
	/// <value>選択操作</value>
	public AbstractScreenHook SelectMenu => this.selectMenu ??= new DelegateScreenHook(ActionSelectMenu);
	/// <summary>
	/// 状態内容を取得します。
	/// </summary>
	/// <value>状態内容</value>
	public string? StatusText {
		get => this.statusText;
		private set => Update(ref this.statusText, value, nameof(StatusText));
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
	/// 選択画面情報を生成します。
	/// </summary>
	/// <param name="headerText">表題内容</param>
	/// <param name="detailText">詳細内容</param>
	/// <param name="sourceData">基本情報</param>
	public StorageDialogData(string headerText, string detailText, string sourceData) {
		this.headerText = headerText;
		this.detailText = detailText;
		this.sourceData = sourceData;
		this.filterText = String.Empty;
		this.filterList = new ObservableCollection<StorageFilterData>(CreateFilterList(this.filterText));
		this.filterCode = -1;
		this.divideData = null;
		this.detailList = new ObservableCollection<StorageDetailData>();
		this.selectData = null;
		this.selectFile = null;
		this.selectMenu = null;
		this.statusText = null;
		this.selectHook = null;
		FilterList = new ReadOnlyObservableCollection<StorageFilterData>(this.filterList);
		DetailList = new ReadOnlyObservableCollection<StorageDetailData>(this.detailList);
	}
	#endregion 生成メソッド定義


	#region 内部メソッド定義
	/// <summary>
	/// 制限位置を検索します。
	/// </summary>
	/// <param name="source">対象情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="result">検索結果</param>
	/// <returns>制限位置が存在した場合、<c>True</c>を返却</returns>
	private static bool ChooseFilterText(string source, int offset, out int result) {
		result = source.IndexOf('|', offset);
		return 0 <= result;
	}
	/// <summary>
	/// 制限一覧を生成します。
	/// </summary>
	/// <param name="source">制限情報</param>
	/// <returns>制限一覧</returns>
	private static List<StorageFilterData> CreateFilterList(string source) {
		var result = new List<StorageFilterData>();
		if (String.IsNullOrEmpty(source) == false) {
			var before = (string?)null;
			foreach (var choose in source.Split('|')) {
				if (before == null) {
					before = choose;
				} else {
					result.Add(new StorageFilterData(before, choose));
					before = null;
				}
			}
			if (before != null) {
				var message = "指定されたフィルター文字列は無効です。" + Environment.NewLine
							+ "フィルター文字列には、フィルターの説明と、その後に縦線およびフィルター パターンが含まれている必要があります。" + Environment.NewLine
							+ "また、複数のフィルターの説明とパターンのペアの間は、縦線で区切る必要があります。" + Environment.NewLine
							+ "フィルターパターンの複数の拡張子は、セミコロンで区切る必要があります。" + Environment.NewLine
							+ "例：\"イメージファイル (*.bmp, *jpg)|*.bmp;*.jpg|すべてのファイル (*.*)|*.*\"";
				throw new ArgumentException(message);
			}
		}
		return result;
	}
	/// <summary>
	/// 制限情報を処理します。
	/// </summary>
	/// <param name="beforeText">変更前情報</param>
	/// <param name="updateText">変更後情報</param>
	private void ActionFilterText(string beforeText, string updateText) {
		List<StorageFilterData> sourceList;
		try {
			sourceList = CreateFilterList(updateText);
		} catch {
			FilterText = beforeText;
			throw;
		}
		this.filterList.Clear();
		foreach (var filterData in sourceList) {
			this.filterList.Add(filterData);
		}
		if (this.filterList.Count <= 0) {
			FilterCode = -1;
		} else {
			FilterCode = 0;
		}
	}
	/// <summary>
	/// 制限情報を抽出します。
	/// </summary>
	/// <param name="result">制限情報</param>
	/// <returns>抽出に成功した場合、<c>True</c>を返却</returns>
	private bool ChooseFilterData([MaybeNullWhen(false)]out StorageFilterData result) {
		if (0 <= this.filterCode && this.filterCode < this.filterList.Count) {
			result = this.filterList[this.filterCode];
			return true;
		} else {
			result = default;
			return false;
		}
	}

	private void ActionFilterCode() {
		if (this.divideData == null) {
			// 処理なし
		} else if (this.divideData.ChooseSelectData(out var result)) {
			ActionDivideData(result);
		}
	}
	/// <summary>
	/// 分類情報を生成します。
	/// </summary>
	/// <returns>分類情報</returns>
	private StorageDivideData CreateDivideData() {
		var sourceData = String.Empty;
		var sourceName = "ROOT";
		if (String.IsNullOrEmpty(this.sourceData)) {
			var moduleData = new RootModule(ActionDivideData);
			var resultData = new StorageDivideData(sourceData, sourceName, moduleData.Choose);
			return resultData;
		} else {
			var moduleData = new PathModule(String.Empty, new DirectoryInfo(this.sourceData), ActionDivideData);
			var resultData = new StorageDivideData(sourceData, sourceName, moduleData.Choose);
			return resultData;
		}
	}
	/// <summary>
	/// 分類情報を処理します。
	/// </summary>
	/// <param name="source">分類情報</param>
	private void ActionDivideData(StorageDivideData source) {
		this.detailList.Clear();
		try {
			var uniqueList = new List<string>();
			if (ChooseFilterData(out var filterData) == false) {
				foreach (var chooseData in Directory.GetFiles(Path.Combine(this.sourceData, source.SourceData))) {
					uniqueList.Add(chooseData);
				}
			} else {
				foreach (var filterText in filterData.SourceList) {
					foreach (var chooseData in Directory.GetFiles(Path.Combine(this.sourceData, source.SourceData), filterText)) {
						if (uniqueList.Contains(chooseData) == false) {
							uniqueList.Add(chooseData);
						}
					}
				}
			}
			uniqueList.Sort();
			foreach (var chooseData in uniqueList) {
				var chooseFile = new FileInfo(chooseData);
				var sourceData = Path.Combine(source.SourceData, chooseFile.Name);
				var sourceName = chooseFile.Name;
				var sourceTime = chooseFile.LastWriteTime;
				var sourceSize = chooseFile.Length;
				this.detailList.Add(new StorageDetailData(sourceData, sourceName, sourceTime, sourceSize));
			}
		} catch {
			// 処理なし
		}
	}
	/// <summary>
	/// 選択情報を抽出します。
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
	/// 選択情報を生成します。
	/// </summary>
	/// <param name="source">選択情報</param>
	/// <param name="result">選択情報</param>
	/// <returns>生成に成功した場合、<c>True</c>を返却</returns>
	private static bool CreateSelectFile(string source, [MaybeNullWhen(false)]out FileInfo result) {
		result = new FileInfo(source);
		return result.Exists;
	}
	/// <summary>
	/// 選択情報を判定します。
	/// </summary>
	/// <param name="select">選択情報</param>
	/// <param name="source">基本情報</param>
	/// <returns>正常である場合、<c>True</c>を返却</returns>
	private static bool AcceptSelectFile(FileInfo select, string source) {
		var choose = select.FullName;
		return choose.StartsWith(source);
	}
	/// <summary>
	/// 選択操作を処理します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	private void ActionSelectMenu(object? parameter) {
		if (ChooseSelectData(parameter?.ToString()) != true) {
			StatusText = null;
			SelectFile = null;
			this.selectHook?.Invoke(this, EventArgs.Empty);
		} else if (String.IsNullOrEmpty(this.selectData)) {
			StatusText = "ファイルを指定してください。";
		} else if (CreateSelectFile(Path.Combine(this.sourceData, this.selectData), out var selectFile) == false) {
			StatusText = "ファイルが存在しません。";
		} else if (AcceptSelectFile(selectFile, this.sourceData) == false) {
			StatusText = "ファイルが存在しません。";
		} else {
			StatusText = null;
			SelectFile = selectFile;
			this.selectHook?.Invoke(this, EventArgs.Empty);
		}
	}
	#endregion 内部メソッド定義

	#region 非公開クラス定義
	/// <summary>
	/// 抽出処理インターフェースです。
	/// </summary>
	private interface BaseModule {
		/// <summary>
		/// 要素情報を抽出します。
		/// </summary>
		/// <returns>要素集合</returns>
		IEnumerable<StorageDivideData> Choose();
	}
	/// <summary>
	/// フォルダ抽出処理クラスです。
	/// </summary>
	private sealed class PathModule : BaseModule {
		/// <summary>
		/// 要素内容
		/// </summary>
		private readonly string sourceText;
		/// <summary>
		/// 基本情報
		/// </summary>
		private readonly DirectoryInfo sourceData;
		/// <summary>
		/// 選択処理
		/// </summary>
		private readonly Action<StorageDivideData> selectHook;

		/// <summary>
		/// フォルダ抽出処理を生成します。
		/// </summary>
		/// <param name="sourceText">要素内容</param>
		/// <param name="sourceData">基本情報</param>
		/// <param name="selectHook">選択処理</param>
		public PathModule(string sourceText, DirectoryInfo sourceData, Action<StorageDivideData> selectHook) {
			this.sourceText = sourceText;
			this.sourceData = sourceData;
			this.selectHook = selectHook;
		}

		/// <summary>
		/// 選択操作を処理します。
		/// </summary>
		/// <param name="source">発信情報</param>
		/// <param name="option">引数情報</param>
		private void ActionSelectData(object? source, EventArgs option) {
			if (source is StorageDivideData choose) {
				this.selectHook(choose);
			}
		}

		/// <summary>
		/// 要素情報を抽出します。
		/// </summary>
		/// <returns>要素集合</returns>
		public IEnumerable<StorageDivideData> Choose() {
			foreach (var chooseData in this.sourceData.GetDirectories()) {
				var sourceData = Path.Combine(this.sourceText, chooseData.Name);
				var sourceName = chooseData.Name;
				var moduleData = new PathModule(sourceData, chooseData, this.selectHook);
				var resultData = new StorageDivideData(sourceData, sourceName, moduleData.Choose);
				resultData.SelectHook += ActionSelectData;
				yield return resultData;
			}
		}
	}
	/// <summary>
	/// ドライブ抽出処理クラスです。
	/// </summary>
	private sealed class RootModule : BaseModule {
		/// <summary>
		/// 選択処理
		/// </summary>
		private readonly Action<StorageDivideData> selectHook;

		/// <summary>
		/// ドライブ抽出処理を生成します。
		/// </summary>
		/// <param name="selectHook">選択処理</param>
		public RootModule(Action<StorageDivideData> selectHook) => this.selectHook = selectHook;

		/// <summary>
		/// 選択操作を処理します。
		/// </summary>
		/// <param name="source">発信情報</param>
		/// <param name="option">引数情報</param>
		private void ActionSelectData(object? source, EventArgs option) {
			if (source is StorageDivideData choose) {
				this.selectHook(choose);
			}
		}

		/// <summary>
		/// 要素情報を抽出します。
		/// </summary>
		/// <returns>要素集合</returns>
		public IEnumerable<StorageDivideData> Choose() {
			foreach (var chooseData in DriveInfo.GetDrives()) {
				var sourceData = chooseData.Name;
				var sourceName = sourceData;
				var moduleData = new PathModule(sourceData, chooseData.RootDirectory, this.selectHook);
				var resultData = new StorageDivideData(sourceData, sourceName, moduleData.Choose);
				resultData.SelectHook += ActionSelectData;
				yield return resultData;
			}
		}
	}
	#endregion 非公開クラス定義
}
