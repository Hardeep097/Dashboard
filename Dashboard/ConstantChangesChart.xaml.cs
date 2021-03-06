﻿using CodeClinic;
using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for ConstantChangesChart.xaml
    /// </summary>
    public partial class ConstantChangesChart : UserControl, INotifyPropertyChanged
    {


        public ConstantChangesChart()
        {
            InitializeComponent();
            IsEfficiency.Configuration = Mappers.Xy<FactoryTelemetry>().X(ft => ft.TimeStamp.Ticks).Y(ft => ft.Efficiency);
            DataContext = this;
        }

        public ChartValues<FactoryTelemetry> ChartsValues { get; set; } = new ChartValues<FactoryTelemetry>();

        private bool readingData = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!readingData)
            {
                Task.Factory.StartNew(ReadData);
            }
            readingData = !readingData;
        }

        private void ReadData()
        {
            //todo : populate the collectioon chartvalue

            string filename = @"C:\Users\hsingh\Desktop\C# Revision\Ex_Files_Code_Clinic_C_Sharp\Exercise Files\Ch06\dashBoardData.csv";

            foreach (var ft in FactoryTelemetry.Load(filename))
            {
                ChartsValues.Add(ft);

                this.EngineEfficiency = ft.Efficiency;

                if (ChartsValues.Count > 30)
                {
                    ChartsValues.RemoveAt(0);
                }
                Thread.Sleep(30);
            }


        }

        private double _EngineEfficiency = 65;
        public double EngineEfficiency
        {
            get
            {
                return _EngineEfficiency;
            }
            set
            {
                _EngineEfficiency = value;
                OnPropertyChanged(nameof(EngineEfficiency));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
