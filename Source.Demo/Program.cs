using System;
using System.Windows;

namespace Otchitta.Demo;

/// <summary>
/// 実行処理クラスです。
/// </summary>
internal sealed class Program : System.Windows.Application {
	/// <summary>
	/// 実行処理クラスを生成します。
	/// </summary>
	private Program() {
		//InitializeComponent();
	}

	/// <summary>
	/// 設定情報を登録します。
	/// </summary>
	/// <param name="source">設定情報</param>
	private void Regist(params ResourceDictionary[] source) {
		foreach (var choose in source) {
			Resources.MergedDictionaries.Add(choose);
		}
	}

	/// <summary>
	/// サンプルプログラムを実行します。
	/// </summary>
	[STAThread]
	public static void Main() {
		// 初期定義：処理なし

		// 設定処理
		var source = new Program();
		source.Regist(
			// ライブラリ定義
			new ResourceDictionary() { Source = new Uri("/Screen/Common/ConfirmDialogData.xaml",        UriKind.Relative) },
			new ResourceDictionary() { Source = new Uri("/Screen/Common/WarningDialogData.xaml",        UriKind.Relative) },
			// プログラム定義
			new ResourceDictionary() { Source = new Uri("/Screen/Dialog/ConfirmScreenData.xaml", UriKind.Relative) },
			new ResourceDictionary() { Source = new Uri("/Screen/Dialog/WarningScreenData.xaml", UriKind.Relative) }
		);
		source.StartupUri = new Uri("/Screen/MainScreenView.xaml", UriKind.Relative);

		// 実行処理
		source.Run();
	}
}
