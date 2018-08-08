using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace SpecificationToSummary
{
    public class ViewModelMainWindow : ViewModelBase
    {
        private readonly ModelMainWindow _modelMainWindow;
        private readonly object _lock = new object();

        private ObservableCollection<FileExcel> _collectionFiles;
        public ObservableCollection<FileExcel> CollectionFiles
        {
            get => _collectionFiles;
            set => Set("CollectionFiles", ref _collectionFiles, value);
        }

        private ObservableCollection<DataFind> _collectionResults = new ObservableCollection<DataFind>();
        public ObservableCollection<DataFind> CollectionResults
        {
            get => _collectionResults;
            set => Set("CollectionResults", ref _collectionResults, value);
        }

        private IList _selectedItems;
        public IList SelectedItems
        {
            get => _selectedItems;
            set => Set("SelectedItems", ref _selectedItems, value);
        }

        private ICollectionView _collectionView;
        public ICollectionView CollectionView
        {
            get => _collectionView;
            set => Set("CollectionView", ref _collectionView, value);
        }

        private ProgressReport _progressReport;
        public ProgressReport ProgressReport
        {
            get => _progressReport;
            set => Set("ProgressReport", ref _progressReport, value);
        }

        private RelayCommand _commandAddFiles;
        public RelayCommand CommandAddFiles
        {
            get
            {
                return _commandAddFiles
                    ?? (_commandAddFiles = new RelayCommand(
                    () =>
                    {
                        var list = _modelMainWindow.GetFiles();
                        if (list != null)
                        {
                            if (_collectionFiles == null || !_collectionFiles.Any())
                            {
                                CollectionFiles = new ObservableCollection<FileExcel>(list);
                            }
                            else
                            {
                                foreach (var item in list)
                                {
                                    CollectionFiles.Add(item);
                                }
                            }
                        }
                    }));
            }
        }

        private RelayCommand _commandClearCollection;
        public RelayCommand CommandClearCollection
        {
            get
            {
                return _commandClearCollection
                    ?? (_commandClearCollection = new RelayCommand(
                    () =>
                    {
                        CollectionFiles.Clear();
                    },
                    () => _collectionFiles?.Count > 0
                    && (_progressReport == null || _progressReport.ProcessType != ProcessType.Working)));
            }
        }

        private RelayCommand _commandClearCollectionResult;
        public RelayCommand CommandClearCollectionResult
        {
            get
            {
                return _commandClearCollectionResult
                    ?? (_commandClearCollectionResult = new RelayCommand(
                    () =>
                    {
                        CollectionResults.Clear();
                        ProgressReport = null;
                    },
                    () => CollectionResults?.Count > 0));
            }
        }

        private RelayCommand _commandReadFile;
        public RelayCommand CommandReadFile
        {
            get
            {
                return _commandReadFile
                    ?? (_commandReadFile = new RelayCommand(
                    () =>
                    {
                        DataRead dataRead = new DataRead()
                        {
                            CollectionFiles = _collectionFiles,
                            CollectionResults = _collectionResults,
                        };

                        ProgressReport = new ProgressReport()
                        {
                            ProgressMax = _collectionFiles.Count,
                            ProcessType = ProcessType.Working
                        };

                        _modelMainWindow.ReadFile(dataRead, ProgressReport);
                    },
                    () => _collectionFiles?.Count > 0
                    && (_progressReport == null || _progressReport.ProcessType != ProcessType.Working)
                    ));
            }
        }

        private RelayCommand _commandEditFile;
        public RelayCommand CommandEditFile
        {
            get
            {
                return _commandEditFile
                    ?? (_commandEditFile = new RelayCommand(
                    () =>
                    {
                        DataEdit dataEdit = new DataEdit()
                        {
                            CollectionResults = _collectionResults
                        };

                        ProgressReport = new ProgressReport();

                        _modelMainWindow.EditFile(dataEdit, _progressReport);
                    },
                    () => _collectionResults?.Count > 0));
            }
        }

        private RelayCommand<object> _commandSelectionChanged;
        public RelayCommand<object> CommandSelectionChanged
        {
            get
            {
                return _commandSelectionChanged
                    ?? (_commandSelectionChanged = new RelayCommand<object>(
                    p =>
                    {
                        if (p is IList list)
                        {
                            SelectedItems = list;
                        }
                    }));
            }
        }

        private RelayCommand _commandMerger;
        public RelayCommand CommandMerger
        {
            get
            {
                return _commandMerger
                    ?? (_commandMerger = new RelayCommand(
                    () =>
                    {
                        if (_selectedItems?.Count == 2)
                        {
                            DataMerger dataMerger = new DataMerger()
                            {
                                CollectionResults = _collectionResults,
                                SelectedItems = _selectedItems
                            };

                            _modelMainWindow.MergerData(dataMerger);
                        }
                    }));
            }
        }

        private RelayCommand _commandUncheck;
        public RelayCommand CommandUncheck
        {
            get
            {
                return _commandUncheck
                    ?? (_commandUncheck = new RelayCommand(
                    () =>
                    {
                        if (_selectedItems != null)
                        {
                            foreach (var item in _selectedItems)
                            {
                                if (item is DataFind a)
                                {
                                    a.IsCheck = !a.IsCheck;
                                }
                            }
                        }
                    }));
            }
        }

        private RelayCommand _commandRemoveItem;
        public RelayCommand CommandRemoveItem
        {
            get
            {
                return _commandRemoveItem
                    ?? (_commandRemoveItem = new RelayCommand(
                    () =>
                    {
                        for (int i = _selectedItems.Count - 1; i >= 0; i--)
                        {
                            var a = _selectedItems[i] as DataFind;
                            _collectionResults.Remove(a);
                        }
                    }));
            }
        }

        public ViewModelMainWindow()
        {
            _modelMainWindow = new ModelMainWindow();
            CollectionView = CollectionViewSource.GetDefaultView(_collectionResults);
            _collectionView.GroupDescriptions.Add(new PropertyGroupDescription("NameFile"));
            BindingOperations.EnableCollectionSynchronization(_collectionResults, _lock);
        }
    }
}