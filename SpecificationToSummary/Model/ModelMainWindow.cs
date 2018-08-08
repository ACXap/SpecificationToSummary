using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificationToSummary
{
    /// <summary>
    /// Класс для работы с данными
    /// </summary>
    public class ModelMainWindow
    {
        /// <summary>
        /// _nameAndCharacteristic - строка для поиска столбца с наименованием позиций
        /// </summary>
        private const string _nameAndCharacteristic = "Наименование и техническая характеристика";
        /// <summary>
        /// _positionFull - строка для поиска столбца с номером позиций
        /// </summary>
        private const string _positionFull = "Позиция";
        /// <summary>
        /// _positionReduced - строка для поиска столбца с номером позиций, дополнительная для старых спецификаций
        /// </summary>
        private const string _positionReduced = "Поз.";
        /// <summary>
        /// _fileResulte - имя файла шаблона для итогового файла
        /// </summary>
        private const string _fileResulte = "Итог.xlsx";
        /// <summary>
        /// _file - имя итогового файла, куда будут занесены все данные
        /// </summary>
        private const string _file = "Итог!!!.xlsx";

        /// <summary>
        /// Метод для получения списка файлов спецификаций
        /// возможен множественный выбор и только файлов .xlsx
        /// </summary>
        /// <returns>Возвращает коллекцию файлов List<FileExcel></returns>
        public List<FileExcel> GetFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Excel Worksheets|*.xlsx"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var list = new List<FileExcel>(openFileDialog.FileNames.Length);
                list.AddRange(openFileDialog.FileNames.Select(p => new FileExcel() { Name = Path.GetFileName(p), Path = p }));
                return list;
            }

            return null;
        }

        /// <summary>
        /// Метод для чтения файлов спецификаций и выбор позиций в них
        /// </summary>
        /// <param name="dataRead">Данные для чтения, коллекция файлов и коллекция для результатов</param>
        /// <param name="progressReport">Данные о прогрессе работы</param>
        public void ReadFile(DataRead dataRead, ProgressReport progressReport)
        {
            Task task = new Task(() =>
            {
                try
                {
                    foreach (var file in dataRead.CollectionFiles)
                    {
                        progressReport.ProcessedFile = file.Name;
                        progressReport.ProgressValue = dataRead.CollectionFiles.IndexOf(file) + 1;

                        using (var fileStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var excel = new XLWorkbook(fileStream))
                            {
                                var list = excel.FindCells(s => s.Value.ToString() == _nameAndCharacteristic);

                                int indexPosition = 0;
                                DataFind data = null;

                                foreach (var cell in list)
                                {
                                    var address = cell.Address;
                                    var cellleft = address.Worksheet.Cell(address.RowNumber, address.ColumnNumber - 1);
                                    if (cellleft.Value.ToString() == _positionFull || cellleft.Value.ToString() == _positionReduced)
                                    {
                                        indexPosition++;

                                        var rowsInList = indexPosition == 1 ? 24 : 32;

                                        for (int k = 0; k < rowsInList; k++)
                                        {
                                            var cellPosition = address.Worksheet.Cell(address.RowNumber + 6 + k, address.ColumnNumber);

                                            if (data == null)
                                            {
                                                data = GetData(cellPosition);
                                                data.NameFile = Path.GetFileNameWithoutExtension(file.Path);
                                                data.IsCheck = true;

                                                if (IsRowEmpty(data))
                                                {
                                                    data = null;
                                                    continue;
                                                }

                                                dataRead.CollectionResults.Add(data);
                                            }
                                            else
                                            {
                                                DataFind tempData = GetData(cellPosition);

                                                if (IsRowEmpty(tempData))
                                                {
                                                    data = null;
                                                    tempData = null;
                                                    continue;
                                                }

                                                SumData(data, tempData);
                                            }
                                        }
                                        data = null;
                                    }
                                }
                            }
                        }
                    }

                    progressReport.ProcessType = ProcessType.Completed;
                    progressReport.ProcessedMessage = "Выполнение чтения завершено!";
                }
                catch (Exception ex)
                {
                    progressReport.ProcessType = ProcessType.Error;
                    progressReport.ProcessedMessage = ex.Message;
                }
            });
            task.Start();
        }

        /// <summary>
        /// Метод для записи полученных позиций в итоговый файл
        /// </summary>
        /// <param name="dataEdit">Данные для записи, коллекция с найденными позициями</param>
        /// <param name="progressReport">Данные о прогрессе работы</param>
        public void EditFile(DataEdit dataEdit, ProgressReport progressReport)
        {
            try
            {
                progressReport.ProgressMax = 1;
                progressReport.ProgressValue = 1;
                progressReport.ProcessedFile = _fileResulte;

                using (var excel = new XLWorkbook(_fileResulte))
                {
                    int indexRow = 4;
                    var worksSheets = excel.Worksheets.First();

                    foreach (var item in dataEdit.CollectionResults)
                    {
                        if (!item.IsCheck)
                        {
                            continue;
                        }
                        worksSheets.Cell(indexRow, "b").Value = item.Code;
                        worksSheets.Cell(indexRow, "d").Style.Alignment.WrapText = true;
                        worksSheets.Cell(indexRow, "d").Value = item.NamePosition;
                        worksSheets.Cell(indexRow, "e").Style.Alignment.WrapText = true;
                        worksSheets.Cell(indexRow, "e").Value = item.TypePosition;
                        worksSheets.Cell(indexRow, "g").Value = item.Unit;

                        if (Double.TryParse(item.Mass.Replace('.', ','), out double mass))
                        {
                            worksSheets.Cell(indexRow, "h").Value = mass / 1000;
                        }
                        else
                        {
                            worksSheets.Cell(indexRow, "h").Value = $"Проверить значение {item.Mass} в файле!!! ";
                            worksSheets.Cell(indexRow, "h").Style.Fill.BackgroundColor = XLColor.Red;
                        }

                        worksSheets.Cell(indexRow, "i").Value = item.Count;
                        worksSheets.Cell(indexRow, "p").Value = item.Factory;
                        worksSheets.Cell(indexRow, "q").Value = item.NameFile;

                        indexRow++;
                    }
                    worksSheets.Column("d").Style.Alignment.WrapText = true;

                    worksSheets.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksSheets.RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    excel.SaveAs(_file);

                    progressReport.ProcessType = ProcessType.Completed;
                    progressReport.ProcessedMessage = "Выполнение записи завершено!";

                    Process.Start(new ProcessStartInfo("explorer", string.Format("/e, /select, \"{0}\"", Path.GetFullPath(_file))));

                }
            }
            catch (Exception ex)
            {
                progressReport.ProcessType = ProcessType.Error;
                progressReport.ProcessedMessage = ex.Message;
            }
        }

        /// <summary>
        /// Метод для объединения позиций в таблице позиций
        /// </summary>
        /// <param name="dataMerger">Данные для объединения, коллекция с найденными позициями и коллекция выделеных позиций</param>
        public void MergerData(DataMerger dataMerger)
        {
            var a = dataMerger.SelectedItems[0] as DataFind;
            var b = dataMerger.SelectedItems[1] as DataFind;

            if (a.NameFile != b.NameFile)
            {
                return;
            }

            if (dataMerger.CollectionResults.IndexOf(a) > dataMerger.CollectionResults.IndexOf(b))
            {
                SumData(b, a);
                dataMerger.CollectionResults.Remove(a);
            }
            else
            {
                SumData(a, b);
                dataMerger.CollectionResults.Remove(b);
            }
        }

        #region Private Method

        /// <summary>
        /// Метод для объединения данных по двум позициям
        /// </summary>
        /// <param name="data">1 позиция</param>
        /// <param name="tempData">2 позиция</param>
        private void SumData(DataFind data, DataFind tempData)
        {
            data.Position += tempData.Position;
            data.NamePosition += tempData.NamePosition;
            data.TypePosition += tempData.TypePosition;
            data.Code += tempData.Code;
            data.Factory += tempData.Factory;
            data.Unit += tempData.Unit;
            data.Count += tempData.Count;
            data.Mass += tempData.Mass;
        }

        /// <summary>
        /// Метод для определения пустой строки в файле спецификации
        /// </summary>
        /// <param name="tempData">Данные строки в файле спецификации</param>
        /// <returns></returns>
        private bool IsRowEmpty(DataFind tempData)
        {
            return tempData.NamePosition == "" && tempData.TypePosition == "" && tempData.Factory == "" && tempData.Unit == "" && tempData.Count == "" && tempData.Mass == "" && tempData.Code == "";
        }

        /// <summary>
        /// Метод для получения первоночальных данных о позиции 
        /// </summary>
        /// <param name="cellPosition">Ячейка с наименованием позиции</param>
        /// <returns>Возвращает данные о позиции</returns>
        private DataFind GetData(IXLCell cellPosition)
        {
            return new DataFind()
            {
                Position = cellPosition.CellLeft().Value.ToString(),
                NamePosition = cellPosition.Value.ToString(),
                TypePosition = cellPosition.CellRight().Value.ToString(),
                Code = GetCode(cellPosition),
                Factory = cellPosition.CellRight(3).Value.ToString(),
                Unit = cellPosition.CellRight(4).Value.ToString(),
                Count = cellPosition.CellRight(5).Value.ToString(),
                Mass = cellPosition.CellRight(6).Value.ToString(),
            };
        }

        /// <summary>
        /// Метод для получения кода оборудования, изделия, марки
        /// </summary>
        /// <param name="cellPosition">Ячейка с наименованием позиции</param>
        /// <returns>Возвращает кода оборудования, изделия, марки</returns>
        private string GetCode(IXLCell cellPosition)
        {
            var code = cellPosition.CellRight(2).RichText;
            string dataCode = string.Empty;
            if (code.Count > 0)
            {
                dataCode = code.First().Text;
            }

            return dataCode;
        }

        #endregion Private Method
    }
}