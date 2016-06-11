using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// アセンブリに関する一般情報は以下の属性セットをとおして制御されます。
// アセンブリに関連付けられている情報を変更するには、
// これらの属性値を変更してください。
[assembly: AssemblyTitle("Akalib.Wpf")]
[assembly: AssemblyDescription("Akatsuki WPF")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("atachimiko")]
[assembly: AssemblyProduct("Akalib.Wpf")]
[assembly: AssemblyCopyright("Copyright ©atachi 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible を false に設定すると、その型はこのアセンブリ内で COM コンポーネントから
// 参照不可能になります。COM からこのアセンブリ内の型にアクセスする場合は、
// その型の ComVisible 属性を true に設定してください。
[assembly: ComVisible(false)]


[assembly: ThemeInfo(
	ResourceDictionaryLocation.None, //テーマ固有のリソース ディクショナリが置かれている場所
	//(リソースがページ、
	//またはアプリケーション リソース ディクショナリに見つからない場合に使用されます)
	ResourceDictionaryLocation.SourceAssembly //汎用リソース ディクショナリが置かれている場所
	//(リソースがページ、
	//アプリケーション、またはいずれのテーマ固有のリソース ディクショナリにも見つからない場合に使用されます)
)]


// 次の GUID は、このプロジェクトが COM に公開される場合の、typelib の ID です
[assembly: Guid("cfc57aa5-ffcb-4e6b-903a-de20e073c537")]

// アセンブリのバージョン情報は、以下の 4 つの値で構成されています:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// すべての値を指定するか、下のように '*' を使ってビルドおよびリビジョン番号を
// 既定値にすることができます:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.3.0")]
[assembly: AssemblyFileVersion("1.0.3.0")]
[assembly: XmlnsDefinition("http://schemas.akalib.net/wpf", "Akalib.Wpf")]
[assembly: XmlnsDefinition("http://schemas.akalib.net/wpf", "Akalib.Wpf.Behaviour")]
[assembly: XmlnsDefinition("http://schemas.akalib.net/wpf", "Akalib.Wpf.Converter")]
[assembly: XmlnsDefinition("http://schemas.akalib.net/wpf", "Akalib.Wpf.Control")]
[assembly: XmlnsDefinition("http://schemas.akalib.net/wpf", "Akalib.Wpf.Control.Tree")]
[assembly: XmlnsPrefix("http://schemas.akalib.net/wpf", "aka")]