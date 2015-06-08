using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EducServLib
{
    /// <summary>
    /// Класс формы для отображения индикатора выполнения
    /// </summary>
    public partial class ProgressForm : Form
    {
        private int _min_prBar_value;
        private int _max_prBar_value;
        private int _prBar_step;
        private int _prBar_value;
        private ProgressBarStyle _prBar_style;
        private string _labelText;
        private string _labelTextBeforeNumberCompleted;
        private string _labelTextAfterNumberCompleted;

        /// <summary>
        /// Создаёт форму для отображения индикатора выполнения со значениями по умолчанию
        /// </summary>
        public ProgressForm()
        {
            InitializeComponent();
            _min_prBar_value = 0;
            _max_prBar_value = 100;
            _prBar_step = 1;
            _prBar_value = _min_prBar_value;
            _prBar_style = ProgressBarStyle.Continuous;
            prBar.Minimum = _min_prBar_value;
            prBar.Maximum = _max_prBar_value;
            prBar.Step = _prBar_step;
            prBar.Value = _prBar_value;
            prBar.Style = _prBar_style;
            _labelText = string.Empty;
            _labelTextBeforeNumberCompleted = string.Empty;
            _labelTextAfterNumberCompleted = string.Empty;
            lbProgress.Text = _labelText;
            this.Refresh();
        }
        /// <summary>
        /// Создаёт форму для отображения индикатора выполнения
        /// с заданными минимальным и максимальным значениями индикатора,
        /// шагом и стилем индикатора
        /// </summary>
        /// <param name="min_prBar_value">Минимальное значение индикатора выполнения</param>
        /// <param name="max_prBar_value">Максимальное значение индикатора выполнения</param>
        /// <param name="prBar_step">Шаг индикатора выполнения</param>
        /// <param name="prBar_style">Стиль индикатора выполнения</param>
        public ProgressForm(int min_prBar_value, int max_prBar_value,
                            int prBar_step, ProgressBarStyle prBar_style)
            : this()
        {
            this._min_prBar_value = min_prBar_value;
            this._max_prBar_value = max_prBar_value;
            this._prBar_step = prBar_step;
            this._prBar_style = prBar_style;
            this.prBar.Minimum = _min_prBar_value;
            this.prBar.Maximum = _max_prBar_value;
            this.prBar.Step = _prBar_step;
            this.prBar.Style = _prBar_style;
            this.Refresh();
        }
        /// <summary>
        /// Создаёт форму для отображения индикатора выполнения
        /// с заданными минимальным и максимальным значениями индикатора,
        /// шагом и стилем индикатора, текстом надписи над индикатором
        /// </summary>
        /// <param name="min_prBar_value">Минимальное значение индикатора выполнения</param>
        /// <param name="max_prBar_value">Максимальное значение индикатора выполнения</param>
        /// <param name="prBar_step">Шаг индикатора выполнения</param>
        /// <param name="prBar_style">Стиль индикатора выполнения</param>
        /// <param name="labelText">Текст надписи над индикатором выполнения</param>
        public ProgressForm(int min_prBar_value, int max_prBar_value,
                            int prBar_step, ProgressBarStyle prBar_style,
                            string labelText)
            : this(min_prBar_value, max_prBar_value,
                   prBar_step, prBar_style)
        {
            this._min_prBar_value = min_prBar_value;
            this._max_prBar_value = max_prBar_value;
            this._prBar_step = prBar_step;
            this._prBar_style = prBar_style;
            this.prBar.Minimum = _min_prBar_value;
            this.prBar.Maximum = _max_prBar_value;
            this.prBar.Step = _prBar_step;
            this.prBar.Style = _prBar_style;
            this._labelText = labelText;
            this.lbProgress.Text = _labelText;
            this.Refresh();
        }
        /// <summary>
        /// Получает или устанавливает минимальное значение индикатора выполнения
        /// </summary>
        public int MinPrBarValue
        {
            get { _min_prBar_value = this.prBar.Minimum; return _min_prBar_value; }
            set { _min_prBar_value = value; this.prBar.Minimum = _min_prBar_value; }
        }
        /// <summary>
        /// Получает или устанавливает максимальное значение индикатора выполнения
        /// </summary>
        public int MaxPrBarValue
        {
            get { _max_prBar_value = this.prBar.Maximum; return _max_prBar_value; }
            set { _max_prBar_value = value; this.prBar.Maximum = _max_prBar_value; }
        }
        /// <summary>
        /// Получает или устанавливает шаг индикатора выполнения
        /// </summary>
        public int PrBarStep
        {
            get { _prBar_step = this.prBar.Step; return _prBar_step; }
            set { _prBar_step = value; this.prBar.Step = _prBar_step; }
        }
        /// <summary>
        /// Получает или устанавливает стиль индикатора выполнения
        /// </summary>
        public ProgressBarStyle PrBarStyle
        {
            get { _prBar_style = this.prBar.Style; return _prBar_style; }
            set { _prBar_style = value; this.prBar.Style = _prBar_style; }
        }
        /// <summary>
        /// Получает или устанавливает текущее значение индикатора выполнения
        /// </summary>
        public int PrBarValue
        {
            get { _prBar_value = this.prBar.Value; return _prBar_value; }
            set { _prBar_value = value; this.prBar.Value = _prBar_value; }
        }
        /// <summary>
        /// Устанавливает части текста в надписи над индикатором выполнения, например:
        /// "Формируется таблица в документе, готово", "из", максимальное значение, "строк"
        /// </summary>
        /// <param name="labelTextBeforeNumberCompleted">Первая часть текста (до изменяющейся величины в надписи),
        /// например: "Формируется таблица в документе, готово"</param>
        /// <param name="labelTextPreposition">Вторая часть текста (предлог перед максимальным значением изменяющейся величины в надписи),
        /// например: "из"</param>
        /// <param name="MaxNumberValue">Максимальное значение изменяющейся величины в надписи,
        /// например, число строк в таблице без учёта строк в её заголовке.
        /// Этот параметр должен быть больше нуля, иначе он не будет отображён в надписи.</param>
        /// <param name="labelTextAfterMaxNumber">Третья часть текста (после максимального значения изменяющейся величины в надписи),
        /// например: "строк"</param>
        public void SetProgressTextByParts(string labelTextBeforeNumberCompleted,
                                           string labelTextPreposition,
                                           int MaxNumberValue,
                                           string labelTextAfterMaxNumber)
        {
            _labelTextBeforeNumberCompleted = labelTextBeforeNumberCompleted == string.Empty ? string.Empty : labelTextBeforeNumberCompleted + " ";
            _labelTextAfterNumberCompleted = " " + (labelTextPreposition == string.Empty ? string.Empty : labelTextPreposition + " ") +
                                             (MaxNumberValue <= 0 ? string.Empty : MaxNumberValue.ToString()) +
                                             (labelTextAfterMaxNumber == string.Empty ? string.Empty : " " + labelTextAfterMaxNumber);
        }
        /// <summary>
        /// Устанавливает надпись над индикатором выполнения
        /// </summary>
        /// <param name="labelText">Текст надписи</param>
        public void SetProgressText(string labelText)
        {
            _labelText = labelText;
            lbProgress.Text = _labelText;
            this.Refresh();
        }
        /// <summary>
        /// Устанавливает надпись над индикатором выполнения,
        /// содержащую информацию об уже обработанном количестве чего-либо
        /// </summary>
        /// <param name="numberCompleted">Уже обработанное количество чего-либо</param>
        public void SetProgressText(int numberCompleted)
        {
            _labelText = _labelTextBeforeNumberCompleted + numberCompleted.ToString() + _labelTextAfterNumberCompleted;
            lbProgress.Text = _labelText;
            this.Refresh();
        }
        /// <summary>
        /// Увеличивает текущее значение индикатора выполнения "ProgressBar" на один шаг.
        /// Величина шага устанавливается свойством формы "PrBarStep"
        /// </summary>
        public void PerformStep()
        {
            if (_prBar_style == ProgressBarStyle.Marquee)
            {
                this.Close();
                string errorText = "Метод \"PerformStep()\" класса \"ProgressForm\" " +
                                   "нельзя использовать, если свойством \"PrBarStyle\" " +
                                   "установлен стиль \"Marquee\" для индикатора выполнения.";
                throw new NotImplementedException(errorText);
            }
            prBar.PerformStep();
        }
    }
}