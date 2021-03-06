using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using GrapeCity.Documents.Drawing;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;

namespace GcPdfWeb.Samples.Basics
{
    // This sample draws some test texts in a number of different languages,
    // including right-to-left and Far Eastern ones.
    // See also PaginatedText.
    public class MultiLang
    {
        // All sizes, distances etc are in points (1/72", GcPdf's default):
        private const float c_CaptionToText = 5;
        private const float c_TextToCaption = 18;
        private const float c_Margin = 36;
        // Private data:
        private GcPdfDocument _doc;
        private TextLayout _captionLayout;
        private TextLayout _textLayout;
        private float _ipY;

        // Method drawing a language's caption and test text:
        private void DrawText(string caption, string text, Font font, bool rtl)
        {
            _captionLayout.Clear();
            _captionLayout.Append(caption);
            _captionLayout.PerformLayout(true);

            _textLayout.Clear();
            _textLayout.DefaultFormat.Font = font;
            _textLayout.RightToLeft = rtl;
            _textLayout.Append(text);
            _textLayout.PerformLayout(true);
            // Add new page if needed:
            GcGraphics g;
            if (_doc.Pages.Count == 0 || _ipY + tlHeight(_captionLayout) + tlHeight(_textLayout) + c_CaptionToText > _doc.PageSize.Height - c_Margin)
            {
                _ipY = c_Margin;
                g = _doc.Pages.Add().Graphics;
            }
            else
                g = _doc.Pages.Last.Graphics;
            // Draw caption:
            g.FillRectangle(new RectangleF(c_Margin, _ipY, _captionLayout.MaxWidth.Value, tlHeight(_captionLayout)), Color.SteelBlue);
            g.DrawTextLayout(_captionLayout, new PointF(c_Margin, _ipY));
            _ipY += tlHeight(_captionLayout);
            _ipY += c_CaptionToText;
            // Draw test text:
            g.DrawRectangle(new RectangleF(c_Margin, _ipY, _textLayout.MaxWidth.Value, tlHeight(_textLayout)), Color.LightSteelBlue, 0.5f);
            g.DrawTextLayout(_textLayout, new PointF(c_Margin, _ipY));
            _ipY += tlHeight(_textLayout);
            _ipY += c_TextToCaption;
            return;

            float tlHeight(TextLayout tl)
            {
                return tl.MarginTop + tl.ContentHeight + tl.MarginBottom;
            }
        }

        public void CreatePDF(Stream stream)
        {
            Font arialbd = Font.FromFile(Path.Combine("Resources", "Fonts", "arialbd.ttf"));
            Font malgun = Font.FromFile(Path.Combine("Resources", "Fonts", "malgun.ttf"));
            Font segoe = Font.FromFile(Path.Combine("Resources", "Fonts", "segoeui.ttf"));
            // Add Arial Unicode MS for Chinese, Hindi and Japanese fallbacks:
            Font arialuni = Font.FromFile(Path.Combine("Resources", "Fonts", "arialuni.ttf"));
            segoe.AddLinkedFont(arialuni);
            malgun.AddLinkedFont(arialuni);
            // Create document:
            _doc = new GcPdfDocument();
            // Init text layouts:
            _captionLayout = new TextLayout()
            {
                MaxWidth = _doc.PageSize.Width - c_Margin * 2,
                MarginLeft = 4,
                MarginRight = 4,
                MarginTop = 2,
                MarginBottom = 2,
            };
            _captionLayout.DefaultFormat.Font = arialbd;
            _captionLayout.DefaultFormat.FontSize = 12;
            _captionLayout.DefaultFormat.ForeColor = Color.AliceBlue;
            //
            _textLayout = new TextLayout()
            {
                FontFallbackScope = FontFallbackScope.None,
                MaxWidth = _doc.PageSize.Width - c_Margin * 2,
                MarginLeft = 6,
                MarginRight = 6,
                MarginTop = 6,
                MarginBottom = 6,
            };
            _textLayout.DefaultFormat.FontSize = 10;
            // Draw texts in a loop:
            Dictionary<string, Font> fonts = new Dictionary<string, Font>() { { "segoe", segoe }, { "malgun", malgun } };
            for (int i = 0; i < s_texts.GetLength(0); ++i)
            {
                string lang = s_texts[i, 0];
                string text = s_texts[i, 1];
                Font font = fonts[s_texts[i, 2]];
                bool rtl = !string.IsNullOrEmpty(s_texts[i, 3]);
                DrawText(lang, text, font, rtl);
            }
            // Done:
            _doc.Save(stream);
        }

        // 0 - Language tag
        // 1 - Test string
        // 2 - Font to use
        // 3 - If not null/empty - the language is RTL
        string[,] s_texts = new string[,]
        {
            {
                "Arabic",
                "?????????????? ???????? ???????? ???????????????? ?????????????? ???? ?????? ?????? ???????????????????? ?????????? ???????? ???????????? ???????????????? ???? ?????????????? ?????????????? ???????? ???? 422 ?????????? ??????????1 ???????????? ???????????????? ???? ?????????????? ???????????????? ???????? ?????????? ?????????????? ???????????????? ?????? ???????????? ???? ?????????????? ???????????? ???????????????? ???????????????? ???????????? ?????????? ?????????? ?????????????????????????????????????????? ?????????????? ?????????? ???????? ?????? ?????????? ?????????????? ???????????????????? ?????? ?????? ?????????? ?????????????? ?????????????????? ???? ??????????????: ?????????????? ?????????????????? ?????????????? ?????????????? ???? ?????????? ?????????? ?????? ?????? ???????????? ???? ?????????????? (?????????????? ????????) ?????? ???????????? ?????? ???? ?????????? ?????? ??????????. ???????????????? ???? ?????????? ?????? ?????????? ???????????? ?????? ?????? ???? ?????????????? ???????????????? ???? ???????????? ?????????????? ?????? ???????? ?????? ???????????? ???? ?????? ?????????????? ?????????????? ???????????????? ???????????????? ???? ???????????? ????????????. ?????????? ???????????? ???????????????? ?????????????? ???????????? ???????????? ?????????? ?????????? ???????????????? ???????????? ?????? ?????????????? ???????????? ???????????? ?????????? ?????????? ???? ?????????????? ???????? ?????????? ?????????????????? ?????????? ???????????????? ?????????????? ?????????????? ???? ?????? ?????????? ?????? ???????? ???? ???????????? ???????????? ???? ???????????? ?????????????????? ???????????????? ?????????????????? ???????????????????????????????????? ?????????????? ?????????????????? ???????????? ?????????????? ?????????????????? ?????? ?????????????? ?????????????????????? ?????????????????? ???????????????????? ???????????????????? ????????????????????.?????? ???????? ???????? ???????? ???????? ???? ?????? ???????? ???? ?????????? ?????????????????? ???????????? ?????????????????? ???????????????? ?????????? ????????????.",
                "segoe",
                "rtl"
            },
            {
                "Belarusian",
                "?????????????????????? ??????????, ???????? ?????????????????? ???????????????????? ?? ??????????? ?????????????????????????????? ??????, ???? ???????????????????? ?????????? ?? ?????????????????????????????????? ?????????? ????????????????, ???? ???????? ?????????????????????? ?? ???????????????? ?? ???? ???????? ??????????, ???????????????? ?????????? ?? ??????????, ??????????????, ??????????????. ??.??. ???????????????? ???????? ?????????????????????? ?? ?????????????????? ???????????????????????? ?? ???????????? ???????????????????????????????????? ???????????? (????.??????????????: ?????????? ?????????? ???????????????????? ???????? ?? ???????????????? ???????????? ?????????????????????????????????? ??????????).",
                "segoe",
                null
            },
            {
                "Chinese",
                "???????????????????????????????????????????????????????????????????????????????????????Chinese??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????[2]???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????",
                "malgun",
                null
            },
            {
                "Czech",
                "??e??tina je z??padoslovansk?? jazyk, nejv??ce p????buzn?? se sloven??tinou, pot?? pol??tinou a lu??ickou srb??tinou. Pat???? mezi slovansk?? jazyky, do rodiny jazyk?? indoevropsk??ch. Vyvinula se ze z??padn??ch n????e???? praslovan??tiny na konci 10. stolet??. ??esky psan?? literatura se objevuje od 14. stolet??. Prvn?? p??semn?? pam??tky jsou v??ak ji?? z 12. stolet??.",
                "segoe",
                null
            },
            {
                "English",
                "English is a West Germanic language originating in England and is the first language for most people in the United Kingdom, the United States, Canada, Australia, New Zealand, Ireland and the Anglophone Caribbean. It is used extensively as a second language and as an official language throughout the world, especially in Commonwealth countries and in many international organisations.",
                "segoe",
                null
            },
            {
                "French",
                "Le fran??ais est une langue romane parl??e principalement en France, dont elle est originaire (la ?? langue d'o??l ??), ainsi qu'au Canada (principalement au Qu??bec, dans le nord et l'est du Nouveau-Brunswick et dans l'est et le nord-est de l'Ontario), en Belgique (en R??gion wallonne et en R??gion de Bruxelles-Capitale) et en Suisse (en Romandie). Le fran??ais est parl?? comme deuxi??me ou troisi??me langue dans d'autres r??gions du monde, comme dans la R??publique d??mocratique du Congo, le plus peupl?? des pays de la francophonie[1] et l'un des 29 [2] pays ayant le fran??ais pour langue officielle ou co-officielle, ou encore au Magreb. Ces pays ayant pour la plupart fait partie des anciens empires coloniaux fran??ais et belge.",
                "segoe",
                null
            },
            {
                "German",
                "Die deutsche Sprache geh??rt zum westlichen Zweig der germanischen Sprachen. Die hochdeutsche Standardsprache wird auch als Standarddeutsch, au??erhalb der sprachwissenschaftlichen Fachsprache als Schriftdeutsch oder einfach Hochdeutsch bezeichnet. Der (zusammenh??ngende) deutsche Sprachraum umfasst viele mitteleurop??ische Staaten (Deutschland, ??sterreich, Belgien (Ostbelgien), Schweiz (Deutschschweiz), Liechtenstein, Luxemburg) und Regionen (S??dtirol, Elsass und Lothringen, zahlreiche Gemeinden Polens) in anderen Staaten. Aufgrund von Auswanderungen werden Deutsch und seine Mundarten auch anderswo gesprochen.",
                "segoe",
                null
            },
            {
                "Greek",
                "?? ???????????????? ???????????????? ???? ?????????????? ???????????? ?????????????? 12 ???????????????????????? ????????????????, ???????????? ???????? ???????????? ?????? ???????? ??????????. ???????????????? ???????????? ?????? ?????????????? ???????????? ???????????????????? ?????????????????? ???????? ??????????????, ?????? ??????????????????, ???????? ???????? ?????? ???????? ??????????????. ???????????????? ?????? ?????????????????????????? ?? ???????????? ???????????????? ?????????? ???? ??????????-?????????????????????? ?????????????????????? ?????????????????? ???????????? ?????? ???????????? ?? ??????????????????, ?? ??????????????, ?? ????????????????, ???? ?????????????? ????????????????, ?? ??????????, ?? ???????????? ?????? ???? ???????????????? ??????????????????. ???????????????? ???????????????????????? ?????? ?? ?????????????????? ?????????????? ???????????????? ?????? ???????????? ???? ???????????????? ?????? ?????????? ?? ?????????????? ???????????? ?????????? ???????? ?????? 20 ??????????????????????.",
                "segoe",
                null
            },
            {
                "Hebrew",
                "?????? ?????? ?????????? ??????\"?? ???????? ???? ?????? ???? ?????????? ??????????. ?????????? \"????????\" ???????? ??????\"?? ?????????? ????????, ???????? ???????? ???? ???????????? ???????? ?????????? ??????????. ???????? ?????????? ???? ?????? ??????\"?? \"???????? ??????????\" (???? \"???????? ??????????\") ?????? ???????????? ???????? ?????????? ????\"?? ???????????? ???? \"???????? ??????????\", ???????? ???????? ?????? ?????????? ???? ??????????. ?????????? ?????? ???????? ?????????? ???????????? ???? ????\"?? ?????????? ???? ???????? ?????????? ???? ???? \"?????? ????????\". ?????????? ?????????????? ?????????? ?????????? ???????? ???????????? ?????? ??????\"??, ???? ???? ???? ???????? ???? ???????? ?????? ???? ????????. ???? ??????, ???????????? ??' ????, ????, ???????????????? ????, ????, ?????????? ???? ?????????? ???????????? ???????? ???????????? ????????????, ?????????? ???? ???????????? ?????? ????????, ???????? ?????? ??\"??????????\" ?????? ??\"????????????\", ?????? ???????? (???????????? ???? ???????? ??????????) ???? ???????? ???? ????????????, ?????????? ?????? ?????? ?????? ???? ????????, ???? ?????????? ?????? ???? ???????? ?????????? ?????????? ??????????????.",
                "segoe",
                "rtl"
            },
            {
                "Hindi",
                "?????????????????? ??????????????????????????? ????????? ?????? ???????????? ?????? ??????????????? ????????????????????? ?????? ?????? ???????????? ?????? ???????????? ?????????????????? ???????????? ?????? ???????????? ???????????????????????? ???????????? ?????? ????????? ?????????????????? ?????? ???????????? ????????????????????? ??????????????? ????????? ???????????? ???????????? ?????? ??????????????? ???????????????????????? ????????? ???????????? ???????????? ????????? ??? ?????? ??????????????? ???????????? ?????? ?????????????????? ?????? ???????????? ?????? ???????????????????????? ???????????? ?????? ??????????????? ???????????? ????????????",
                "segoe",
                null
            },
            {
                "Italian",
                "L'italiano ?? una lingua appartenente al gruppo delle lingue romanze della famiglia delle lingue indoeuropee. Convive con un gran numero di idiomi neo-romanzi e ha delle varianti regionali, per via dell'influenza che su di esso esercitano le lingue regionali. L'italiano ?? lingua ufficiale dell'Italia, di San Marino, della Svizzera (insieme al francese e al tedesco; mentre il romancio ?? lingua nazionale ma ufficiale soltanto nel Canton Grigioni), della Citt?? del Vaticano (insieme al latino) e del Sovrano Militare Ordine di Malta. ?? seconda lingua, coufficiale insieme col croato, nella Regione Istriana (Croazia) e, insieme con lo sloveno, nelle citt?? di Pirano, Isola d'Istria e Capodistria in Slovenia. Pur non figurando tra le lingue parlate in questi paesi, e non essendo quindi utilizzato a livello ufficiale, l'italiano ?? inoltre ampiamente compreso nella restante parte della Venezia Giulia ceduta alla Jugoslavia nel 1947, nel Principato di Monaco, a Malta, in Corsica e nel Nizzardo (Francia) e, in misura minore, in Albania, Libia, Eritrea, Stati Uniti d'America nordorientali, Argentina e Brasile meridionale.",
                "segoe",
                null
            },
            {
                "Japanese",
                "?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????1???3??????????????????????????????????????????????????????????????????????????????????????????????????????????????????",
                "malgun",
                null
            },
            {
                "Korean",
                "?????????(?????????)??? ????????? ???????????? ?????? ???????????? ?????????, ????????????(??????)????????? ?????????, ?????????, ??????(??????)?????? ?????????. ?????????????????????????????????(??????), ??????(???????????????)??? ????????? ??????????????? ?????????, ?????????(?????????)???, ?????????????????? ?????? ?????????????????? ???????????? ??????????????? ?????????(?????????)??? ?????????.",
                "malgun",
                null
            },
            {
                "Portuguese",
                "A l??ngua portuguesa, com mais de 170 milh??es de falantes nativos, ?? a quinta l??ngua mais falada no mundo e a terceira mais falada no mundo ocidental. ?? o idioma oficial de Angola, Brasil, Cabo Verde, Guin??-Bissau, Guin?? Equatorial, Macau, Mo??ambique, Portugal, S??o Tom?? e Pr??ncipe e Timor-Leste, sendo tamb??m falada nos antigos territ??rios da ??ndia Portuguesa (Goa, Dam??o, Diu e Dadr?? e Nagar-Aveli), al??m de ter tamb??m estatuto oficial na Uni??o Europeia, no Mercosul e na Uni??o Africana.",
                "segoe",
                null
            },
            {
                "Russian",
                "?????????????? ???????? ??? ???????? ???? ???????????????????????????????????? ????????????, ???????? ???? ???????????????????? ???????????? ????????, ?? ?????? ?????????? ?????????? ???????????????????????????????? ???? ???????????????????? ???????????? ?? ?????????? ???????????????????????????????? ???????? ????????????, ?????? ??????????????????????????, ?????? ?? ???? ?????????? ?????????????????? ?????????? ?????? ?????????????? (???????? ????????????????????????, ?? ?????????????????????????? ????????????????, ?????????? ???????????????? ?????????????????? ???????????? ?????????????????? ?? ????????).",
                "segoe",
                null
            },
            {
                "Spanish",
                "El espa??ol o castellano es una lengua romance del grupo ib??rico. Desde el punto de vista estrictamente ling????stico, el espa??ol es una familia de cincuenta y ocho lenguas o variedades, que constituyen una cadena de solidaridad ling????stica, con eslabones contiguos o eslabones m??s separados. Es uno de los seis idiomas oficiales de la ONU y, tras el chino mandar??n, es la lengua m??s hablada del mundo por el n??mero de hablantes que la tienen como lengua materna. Es tambi??n idioma oficial en varias de las principales organizaciones pol??tico-econ??micas internacionales (UE, UA, TLCAN y UNASUR, entre otras). Lo hablan como primera y segunda lengua entre 450 y 500 millones de personas, pudiendo ser la segunda lengua m??s hablada considerando los que lo hablan como primera y segunda lengua. Por otro lado, el espa??ol es el segundo idioma m??s estudiado en el mundo tras el ingl??s, con al menos 17,8 millones de estudiantes, si bien otras fuentes indican que se superan los 46 millones de estudiantes distribuidos en 90 pa??ses.",
                "segoe",
                null
            },
            {
                "Thai",
                "????????????????????? ?????????????????????????????????????????????????????????????????????????????? ????????????????????????????????????????????????????????? ???????????????????????????????????????????????????????????????????????????????????? ???????????????????????????????????????????????????????????????????????????????????? ????????????????????????????????????????????????????????????????????????????????????????????????-???????????? ???????????????????????????????????? ????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? ?????????????????????????????????????????????????????????????????????????????????????????? ?????????????????????????????????????????????????????????????????????????????????????????? ????????????????????????????????????????????????-??????????????????????????? ????????????????????????????????????????????????????????????????????? ???????????????????????????????????????-???????????????",
                "segoe",
                null
            },
            {
                "Turkish",
                "T??rk??e, (T??rkiye T??rk??esi olarak da bilinir) Ural-Altay dil ailesine ba??l?? T??rk dillerinden ve O??uz Grubu'na mensup bir dildir. T??rkiye, K??br??s, Balkanlar ve Orta Avrupa ??lkeleri ba??ta olmak ??zere geni?? bir co??rafyada konu??ulmaktad??r. T??rkiye Cumhuriyeti,Kuzey K??br??s T??rk Cumhuriyeti ve K??br??s Cumhuriyeti'nin resm??; Makedonya ve Kosova'n??n ise tan??nm???? b??lgesel dilidir. T??rk??e, farkl?? a????zlara ayr??lm???? bir dildir. Ancak bu a????zlardan ??stanbul leh??esi, sivrile??erek yaz?? dili haline gelmi??tir. T??rk??e, 8 ??nl?? harf say??s??yla beraber zengin bir dil olmas??n??n yan?? s??ra ??zne-nesne-y??klem ??eklindeki c??mle kurulu??lar??yla bilinmektedir.",
                "segoe",
                null
            },
            {
                "Ukrainian",
                "???????????????????? ???????? ???????????????? ???? ???????????????????????????????? ???????????? ????????????, ????????'?????????????? ?????????? ?? ?????????? ?? ???????????????????? ???? ?????????????????????? ???? ????????????????????'?????????????? ????????????????. ???????????????????? ???????????????????????? ???? ?????????????????????? ?? ???????????????????? ????????, ???????? ???????????? ???? ???????? ???????????????? ?????? ?????????????????????????????????? ?? ???????????? ???????????? ??????????????????????, ?????????????????? ?? 15-17 ??????????????. ???????????????????? ???????? ?????? ?????? ?????????? ??????????????????. ???????????????? ????????????????-???????????????? ?????????????? ???????? ???????????????????????? ???????????????????? ?? ???????????????????????? ?????????????????????? ??????????, ???????????????? ?????????????????????? ???????????????????????? ???? ???????????????????????? ?????????????? ???????????????????? ?????????????? ???????????????????? ??????????.",
                "segoe",
                null
            }
        };
    }
}
