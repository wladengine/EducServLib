using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using RtfWriter;


namespace EducServLib
{
    public static class PrintServ
    {
        public static ProgressForm PrepareProgressForm(int rowCount)
        {
            // Создаём форму с индикатором выполнения
            ProgressForm pform = new ProgressForm();
            // Устанавливаем надпись над индикатором выполнения
            pform.SetProgressText("Формируется структура документа ...");
            // Устанавливаем три части текста в надписи над индикатором выполнения
            pform.SetProgressTextByParts("Формируется таблица в документе, готово", "из", rowCount, "строк");
            
            // Устанавливаем максимальное значение индикатора выполнения
            pform.MaxPrBarValue = rowCount;
            // Показываем форму с индикатором выполнения
            pform.Show();

            return pform;
        }
        
        public static RtfDocument PrepareRtfDoc()
        {            
            // ========================================================================================================
            // Создаём документ RTF. См. ПримерРаботыСбиблиотекойRtfWriter.docx и спецификации - Word2007RTFSpec9.docx
            // ========================================================================================================
            // Пересчитываем размер всех полей в документе (20 мм) в размер в пойнтах
            float margin = (float)(20.0m * RtfConstants.MILLIMETERS_TO_POINTS);
            // Создаём документ
            // Размер - A4, ориентация - альбомная, язык по-умолчанию - русский
            RtfDocument doc = new RtfDocument(PaperSize.A4, PaperOrientation.Landscape, Lcid.Russian, margin, margin, margin, margin);

            return doc;
        }

        public static RtfDocument PrepareRtfDocPortrait()
        {
            // ========================================================================================================
            // Создаём документ RTF. См. ПримерРаботыСбиблиотекойRtfWriter.docx и спецификации - Word2007RTFSpec9.docx
            // ========================================================================================================
            // Пересчитываем размер всех полей в документе (20 мм) в размер в пойнтах
            float margin = (float)(20.0m * RtfConstants.MILLIMETERS_TO_POINTS);
            // Создаём документ
            // Размер - A4, ориентация - альбомная, язык по-умолчанию - русский
            RtfDocument doc = new RtfDocument(PaperSize.A4, PaperOrientation.Portrait, Lcid.Russian, margin, margin, margin, margin);

            return doc;
        }

        public static void FillFields(ref RtfDocument doc, float fontSize, Align alignLoc, List<string> lstFields)
        {
            // Создаём абзацы в документе
            foreach (string field in lstFields)
            {
                RtfParagraph par = doc.addParagraph();
                par.Alignment = alignLoc;
                par.DefaultCharFormat.FontSize = fontSize;                
                par.Text = field;                
            }
        }

        public static void FillFields(ref RtfDocument doc, float fontSize, Align alignLoc, string field)
        {            
            RtfParagraph par = doc.addParagraph();
            par.Alignment = alignLoc;
            par.DefaultCharFormat.FontSize = fontSize;
            par.Text = field;            
        }

        // метод для обычной таблицы - строка заголовков, все ячейки обычные
        public static RtfTable PrepareTable(ref RtfDocument doc, ref ProgressForm pform, int rowCount, int colCount)
        {
            // Добавляем таблицу к документу
            // Число строк таблицы равно количеству " rowCount записей плюс строка для заголовка таблицы
            RtfTable table = doc.addTable(rowCount, colCount, (float)(276.1m * RtfConstants.MILLIMETERS_TO_POINTS));
            // Устанавливаем отступ таблицы от левого поля документа (в миллиметрах)
            table.Margins[Direction.Left] = (float)(-5m * RtfConstants.MILLIMETERS_TO_POINTS);            
            pform.PerformStep();
            
            return table; 
        }

        public static void FillCell(this RtfTable table, int row, int column, string text, FontStyleFlag fontStyle)
        {
            table.cell(row, column).Alignment = Align.Center;
            table.cell(row, column).AlignmentVertical = AlignVertical.Middle;
            table.cell(row, column).DefaultCharFormat.FontStyle.addStyle(fontStyle);
            table.cell(row, column).addParagraph().Text = text;
        }

        public static void FillCell(this RtfTable table, int row, int column, string text, FontStyleFlag fontStyle, float fontSize)
        {
            table.cell(row, column).Alignment = Align.Center;
            table.cell(row, column).AlignmentVertical = AlignVertical.Middle;
            table.cell(row, column).DefaultCharFormat.FontStyle.addStyle(fontStyle);
            var p = table.cell(row, column).addParagraph();
            p.DefaultCharFormat.FontSize = fontSize;
            p.Text = text;
        }

        public static void CloseRtfDoc(ref RtfDocument doc, ref RtfTable table, ref ProgressForm pform, string saveTempFolder, string fName)
        {
            pform.SetProgressText("Завершается формирование документа ...");
            // Задаём толщину внутренних границ таблицы
            table.setInnerBorder(RtfWriter.BorderStyle.Single, 0.5f);
            // Задаём толщину внешних границ таблицы
            table.setOuterBorder(RtfWriter.BorderStyle.Single, 0.5f);            

            try
            {
                // Если каталог для сохранения RTF файла не существует, то создаём его
                if (!Directory.Exists(saveTempFolder))
                    Directory.CreateDirectory(saveTempFolder);
                doc.save(saveTempFolder + fName);
                pform.PerformStep();
            }
            catch (Exception ex)
            {
                pform.Close();
                MessageBox.Show("Ошибка при сохранении файла:" +
                                Environment.NewLine + Environment.NewLine + ex + Environment.NewLine + Environment.NewLine +
                                "Закройте файл \"" + fName + "\"," + Environment.NewLine +
                                "затем удалите его из папки " + saveTempFolder + Environment.NewLine +
                                "и нажмите на кнопку \"Печать\" ещё раз.");
            }

            // ==========================================================================
            // Открываем сохранённый RTF файл
            // ==========================================================================
            pform.SetProgressText("Документ открывается ...");
            pform.PerformStep();
            pform.TopMost = false;
            pform.Close();
        }

        public static void CloseRtfDoc(ref RtfDocument doc, ref ProgressForm pform, string saveTempFolder, string fName)
        {
            pform.SetProgressText("Завершается формирование документа ...");
           
            try
            {
                // Если каталог для сохранения RTF файла не существует, то создаём его
                if (!Directory.Exists(saveTempFolder))
                    Directory.CreateDirectory(saveTempFolder);
                doc.save(saveTempFolder + fName);
                pform.PerformStep();
            }
            catch (Exception ex)
            {
                pform.Close();
                MessageBox.Show("Ошибка при сохранении файла:" +
                                Environment.NewLine + Environment.NewLine + ex + Environment.NewLine + Environment.NewLine +
                                "Закройте файл \"" + fName + "\"," + Environment.NewLine +
                                "затем удалите его из папки " + saveTempFolder + Environment.NewLine +
                                "и нажмите на кнопку \"Печать\" ещё раз.");
            }

            // ==========================================================================
            // Открываем сохранённый RTF файл
            // ==========================================================================
            pform.SetProgressText("Документ открывается ...");
            pform.PerformStep();
            pform.TopMost = false;
            pform.Close();
        }
    }
}