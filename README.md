# GC Documents for PDF (GcPdf)

## 简介

**GrapeCity Documents for PDF** （简称 **GcPdf**） 是一个针对.NET Standard 2.0的类库集合，用C＃编写，并提供API，允许您从头创建PDF文件并加载，分析和 修改现有文件。

**GcPdf** 在.NET Standard支持的所有平台上运行，包括.NET Core，ASP.NET Core，.NET Framework等，以及所有操作系统（Windows，Linux和MAC）。

**GcPdf** 和其支持包[可在NuGet.org上获得](https://www.nuget.org/packages?q=grapecity.documents):

- [GrapeCity.Documents.Pdf](https://www.nuget.org/packages/GrapeCity.Documents.Pdf/)
- [GrapeCity.Documents.Barcode](https://www.nuget.org/packages/GrapeCity.Documents.Barcode/)
- [GrapeCity.Documents.Common](https://www.nuget.org/packages/GrapeCity.Documents.Common/)
- [GrapeCity.Documents.Common.Windows](https://www.nuget.org/packages/GrapeCity.Common.Windows/)
- [GrapeCity.Documents.DX.Windows](https://www.nuget.org/packages/GrapeCity.DX.Windows/)

要在应用程序中使用 **GcPdf**  ，您需要引用（安装） **GrapeCity.Documents.Pdf** 包。 它将引入所需的基础架构包。

要渲染条形码，请安装 **GrapeCity.Documents.Barcode** （简称 **GcBarcode**）包。 它提供了一系列的扩展方法，允许在使用 **GcPdf** 时呈现条形码。

在 **Windows** 系统上，您可以选择安装 **GrapeCity.Documents.Common.Windows** 。 它支持Windows注册表中指定的字体链接，以及对本机Windows映像API的访问，提高性能并添加一些功能（例如TIFF支持）。

## 这个存储库包含什么

此存储库包含演示 **GcPdf** 的基本和高级功能的示例项目。 样本代码包含大量注释，可帮助您学习 **GcPdf** 并快速应用它。

## 授权

要在生产环境中使用 **GcPdf**，您需要有效的许可证，有关详细信息，请参阅[GrapeCity许可]（https://www.grapecity.com/en/licensing/grapecity/）。

在没有许可证的情况下使用时，**GcPdf** 具有以下限制:

- 标题表示使用了未经许可的版本，将添加到生成的PDF中所有页面的顶部。
- 加载PDF时，最多只能加载前5页面。

## 更多资源

- [GcPdf主页](https://www.grapecity.com.cn/developer/grapecitydocuments/pdf)
- [GcPdf下载](https://www.grapecity.com.cn/download/?pid=64)
- [使用指南](https://www.grapecity.com.cn/developer/grapecitydocuments/pdf#guide)
- [在线演示](https://demos.componentone.com/gcdocs/gcpdf)
