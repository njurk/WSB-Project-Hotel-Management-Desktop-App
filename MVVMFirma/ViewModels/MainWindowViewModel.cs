using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Helper;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using MVVMFirma.Views;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;

namespace MVVMFirma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        #endregion

        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }
        private List<CommandViewModel> CreateCommands()
        {
            Messenger.Default.Register<string>(this, open);
            // open - metoda zdefiniowana w Helpers

            return new List<CommandViewModel>
            {
                /*new CommandViewModel(
                    "Klasa pokoju",
                    new BaseCommand(() => this.CreateView(new NowyKlasaPokojuViewModel()))),*/

                new CommandViewModel(
                    "Klasy pokojów",
                    new BaseCommand(() => this.ShowAllView(new WszystkieKlasaPokojuViewModel()))),

                /*new CommandViewModel(
                    "Klient",
                    new BaseCommand(() => this.CreateView(new NowyKlientViewModel()))),*/

                new CommandViewModel(
                    "Klienci",
                    new BaseCommand(() => this.ShowAllView(new WszystkieKlientViewModel()))),

                /*new CommandViewModel(
                    "Piętro",
                    new BaseCommand(() => this.CreateView(new NowyPietroViewModel()))),*/

                new CommandViewModel(
                    "Piętra",
                    new BaseCommand(() => this.ShowAllView(new WszystkiePietroViewModel()))),

                /*new CommandViewModel(
                    "Rodzaj pracownika",
                    new BaseCommand(() => this.CreateView(new NowyRodzajPracownikaViewModel()))),*/

                new CommandViewModel(
                    "Rodzaje pracowników",
                    new BaseCommand(() => this.ShowAllView(new WszystkieRodzajPracownikaViewModel()))),

                /*new CommandViewModel(
                    "Sposób płatności",
                    new BaseCommand(() => this.CreateView(new NowySposobPlatnosciViewModel()))),*/

                new CommandViewModel(
                    "Sposoby płatności",
                    new BaseCommand(() => this.ShowAllView(new WszystkieSposobPlatnosciViewModel()))),

                /*new CommandViewModel(
                    "Status płatności",
                    new BaseCommand(() => this.CreateView(new NowyStatusPlatnosciViewModel()))),*/

                new CommandViewModel(
                    "Statusy płatności",
                    new BaseCommand(() => this.ShowAllView(new WszystkieStatusPlatnosciViewModel()))),

                /*new CommandViewModel(
                    "Status pokoju",
                    new BaseCommand(() => this.CreateView(new NowyStatusPokojuViewModel()))),*/

                new CommandViewModel(
                    "Statusy pokojów",
                    new BaseCommand(() => this.ShowAllView(new WszystkieStatusPokojuViewModel()))),

                /*new CommandViewModel(
                    "Typ pokoju",
                    new BaseCommand(() => this.CreateView(new NowyTypPokojuViewModel()))),*/

                new CommandViewModel(
                    "Typy pokojów",
                    new BaseCommand(() => this.ShowAllView(new WszystkieTypPokojuViewModel()))),

                /*new CommandViewModel(
                    "Typ usługi",
                    new BaseCommand(() => this.CreateView(new NowyTypUslugiViewModel()))),*/

                new CommandViewModel(
                    "Typy usług",
                    new BaseCommand(() => this.ShowAllView(new WszystkieTypUslugiViewModel()))),

                /*new CommandViewModel(
                    "Udogodnienie",
                    new BaseCommand(() => this.CreateView(new NowyUdogodnienieViewModel()))),*/

                new CommandViewModel(
                    "Udogodnienia",
                    new BaseCommand(() => this.ShowAllView(new WszystkieUdogodnienieViewModel()))),

                /*new CommandViewModel(
                    "Faktura",
                    new BaseCommand(() => this.CreateView(new NowyFakturaViewModel()))),*/

                new CommandViewModel(
                    "Faktury",
                    new BaseCommand(() => this.ShowAllView(new WszystkieFakturaViewModel()))),

                /*new CommandViewModel(
                    "Rezerwacja",
                    new BaseCommand(() => this.CreateView(new NowyRezerwacjaViewModel()))),*/

                new CommandViewModel(
                    "Rezerwacje",
                    new BaseCommand(() => this.ShowAllView(new WszystkieRezerwacjaViewModel()))),

                /*new CommandViewModel(
                    "Płatność",
                    new BaseCommand(() => this.CreateView(new NowyPlatnoscViewModel()))),*/

                new CommandViewModel(
                    "Płatności",
                    new BaseCommand(() => this.ShowAllView(new WszystkiePlatnoscViewModel()))),

                /*new CommandViewModel(
                    "Pracownik",
                    new BaseCommand(() => this.CreateView(new NowyPracownikViewModel()))),*/

                new CommandViewModel(
                    "Pracownicy",
                    new BaseCommand(() => this.ShowAllView(new WszystkiePracownikViewModel()))),

                /*new CommandViewModel(
                    "Usługa",
                    new BaseCommand(() => this.CreateView(new NowyUslugaViewModel()))),*/

                new CommandViewModel(
                    "Usługi",
                    new BaseCommand(() => this.ShowAllView(new WszystkieUslugaViewModel()))),

                /*new CommandViewModel(
                    "Pokój",
                    new BaseCommand(() => this.CreateView(new NowyPokojViewModel()))),*/

                new CommandViewModel(
                    "Pokoje",
                    new BaseCommand(() => this.ShowAllView(new WszystkiePokojViewModel()))),

                /*new CommandViewModel(
                    "Udogodnienie klasy pokoju",
                    new BaseCommand(() => this.CreateView(new NowyUdogodnieniaKlasPokojuViewModel()))),*/

                new CommandViewModel(
                    "Udogodnienia klas pokojów",
                    new BaseCommand(() => this.ShowAllView(new WszystkieUdogodnieniaKlasPokojuViewModel())))
            };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            //workspace.Dispos();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers
        private void CreateView(WorkspaceViewModel nowy)
        {
            this.Workspaces.Add(nowy);
            this.SetActiveWorkspace(nowy);
        }

        private void ShowAllView(WorkspaceViewModel nowy)
        {
            var workspace = this.Workspaces.FirstOrDefault(vm => vm.GetType() == nowy.GetType());

            if (workspace == null)
            {
                this.Workspaces.Add(nowy);
                workspace = nowy;
            }

            this.SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        private void open(string name) // name - wysyłany komunikat
        {
            if (name == "FakturyAdd")
                CreateView(new NowyFakturaViewModel());
            if (name == "Klasy pokojówAdd")
                CreateView(new NowyKlasaPokojuViewModel());
            if (name == "KlienciAdd")
                CreateView(new NowyKlientViewModel());
            if (name == "PiętraAdd")
                CreateView(new NowyPietroViewModel());
            if (name == "PłatnościAdd")
                CreateView(new NowyPlatnoscViewModel());
            if (name == "PokojeAdd")
                CreateView(new NowyPokojViewModel());
            if (name == "PracownicyAdd")
                CreateView(new NowyPracownikViewModel());
            if (name == "RezerwacjeAdd")
                CreateView(new NowyRezerwacjaViewModel());
            // tu dla rezerwacji pokojów
            if (name == "Rodzaje pracownikówAdd")
                CreateView(new NowyRodzajPracownikaViewModel());
            if (name == "Sposoby płatnościAdd")
                CreateView(new NowySposobPlatnosciViewModel());
            if (name == "Statusy płatnościAdd")
                CreateView(new NowyStatusPlatnosciViewModel());
            if (name == "Statusy pokojówAdd")
                CreateView(new NowyStatusPokojuViewModel());
            if (name == "Typy pokojówAdd")
                CreateView(new NowyTypPokojuViewModel()); 
            if (name == "Typy usługAdd")
                CreateView(new NowyTypUslugiViewModel());
            if (name == "Udogodnienia klas pokojówAdd")
                CreateView(new NowyUdogodnieniaKlasPokojuViewModel());
            if (name == "UdogodnieniaAdd")
                CreateView(new NowyUdogodnienieViewModel());
            if (name == "UsługiAdd")
                CreateView(new NowyUslugaViewModel());
        }
        #endregion
    }
}
