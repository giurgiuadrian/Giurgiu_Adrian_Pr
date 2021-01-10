using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ServiceDBModel;
using System.Data.Entity;
using System.Data;

namespace Giurgiu_Adrian_Pr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    enum ActionState
    {
        New,
        Edit,
        Delete,
        Save,
        Nothing
    }

    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        ServiceDBEntitiesModel ctx = new ServiceDBEntitiesModel();
        CollectionViewSource proprietarViewSource;
        CollectionViewSource intrariViewSource;
        CollectionViewSource proprietarBazaDatesViewSource;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            proprietarViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("proprietarViewSource")));
            proprietarViewSource.Source = ctx.Proprietars.Local;
            proprietarBazaDatesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("proprietarBazaDatesViewSource")));
            //proprietarViewSource.Source = ctx.BazaDates.Local;

            intrariViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("intrariViewSource")));
            intrariViewSource.Source = ctx.Intraris.Local;

            ctx.Proprietars.Load();
            ctx.BazaDates.Load();

            
            ctx.Intraris.Load();

            //System.Windows.Data.CollectionViewSource proprietarViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("proprietarViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // proprietarViewSource.Source = [generic data source]
            //System.Windows.Data.CollectionViewSource intrariViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("intrariViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // intrariViewSource.Source = [generic data source]

            cmbProprietar.ItemsSource = ctx.Proprietars.Local;
            //cmbProprietar.DisplayMemberPath = "Nume";
            cmbProprietar.SelectedValuePath = "idProprietar";

            cmbIntrari.ItemsSource = ctx.Intraris.Local;
            //cmbIntrari.DisplayMemberPath = "Marca";
            cmbIntrari.SelectedValuePath = "idAuto";

            BindDataGrid();
        }

        

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Proprietar proprietar = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    proprietar = new Proprietar()
                    {
                        Nume = numeTextBox.Text.Trim(),
                        Prenume = prenumeTextBox.Text.Trim(),
                        Telefon = telefonTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Proprietars.Add(proprietar);
                    proprietarViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (action == ActionState.Edit)
            {
                try
                {
                    proprietar = (Proprietar)proprietarDataGrid.SelectedItem;
                    proprietar.Nume = numeTextBox.Text.Trim();
                    proprietar.Prenume = prenumeTextBox.Text.Trim();
                    proprietar.Telefon = telefonTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                proprietarViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                proprietarViewSource.View.MoveCurrentTo(proprietar);
            }

            else if (action == ActionState.Delete)
            {
                try
                {
                    proprietar = (Proprietar)proprietarDataGrid.SelectedItem;
                    ctx.Proprietars.Remove(proprietar);
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                proprietarViewSource.View.Refresh();
            }
            SetValidationBinding();
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Proprietar proprietar = null;
            proprietar = new Proprietar()
            {
                Nume = numeTextBox.Text.Trim(),
                Prenume = prenumeTextBox.Text.Trim(),
                Telefon = telefonTextBox.Text.Trim()
            };
            //adaugam entitatea nou creata in context
            ctx.Proprietars.Add(proprietar);

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
                {
                action = ActionState.Edit;
                BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(prenumeTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(telefonTextBox, TextBox.TextProperty);
                SetValidationBinding();
                }

       private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Proprietar proprietar = null;
            proprietar = (Proprietar)proprietarDataGrid.SelectedItem;
            ctx.Proprietars.Remove(proprietar);
        }
        
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            proprietarViewSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            proprietarViewSource.View.MoveCurrentToPrevious();
        }

        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            Intrari intrari = null;
            if (action == ActionState.New)
            {
                try
                {
                    intrari = new Intrari()
                    {
                        Model = modelTextBox.Text.Trim(),
                        Marca = marcaTextBox.Text.Trim(),
                        VIN = vINTextBox.Text.Trim(),
                        KM = kMTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Intraris.Add(intrari);
                    intrariViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (action == ActionState.Edit)
            {
                try
                {
                    intrari = (Intrari)intrariDataGrid.SelectedItem;
                    intrari.Model = modelTextBox.Text.Trim();
                    intrari.Marca = marcaTextBox.Text.Trim();
                    intrari.VIN = vINTextBox.Text.Trim();
                    intrari.KM = kMTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                intrariViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                intrariViewSource.View.MoveCurrentTo(intrari);
            }

            else if (action == ActionState.Delete)
            {
                try
                {
                    intrari = (Intrari)intrariDataGrid.SelectedItem;
                    ctx.Intraris.Remove(intrari);
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                intrariViewSource.View.Refresh();
            }
        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            Intrari intrari = null;
            intrari = new Intrari()
            {
                Marca = marcaTextBox.Text.Trim(),
                Model = modelTextBox.Text.Trim(),
                VIN = vINTextBox.Text.Trim(),
                KM = kMTextBox.Text.Trim()
            };
            //adaugam entitatea nou creata in context
            ctx.Intraris.Add(intrari);
        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(modelTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(marcaTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(vINTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(kMTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            Intrari intrari = null;
            intrari = (Intrari)intrariDataGrid.SelectedItem;
            ctx.Intraris.Remove(intrari);
        }

        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            proprietarViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            proprietarViewSource.View.MoveCurrentToNext();
        }

        private void btnSave0_Click(object sender, RoutedEventArgs e)
        {
            BazaDate bazaDate = null;
            if (action == ActionState.New)
            {
                dynamic selectedOrder = bazaDatesDataGrid.SelectedItem;
                try
                {
                    //int curr_id = selectedOrder.idBaza;
                    //var newBaza = ctx.BazaDates.FirstOrDefault(s => s.idBaza == curr_id);
                    Proprietar proprietar = (Proprietar)cmbProprietar.SelectedItem;
                    Intrari intrari = (Intrari)cmbIntrari.SelectedItem;
                    //instantiem Order entity
                    bazaDate = new BazaDate()
                    {

                        idProprietar = proprietar.idProprietar,
                        idAuto = intrari.idAuto
                    };
                    //adaugam entitatea nou creata in context
                    ctx.BazaDates.Add(bazaDate);
                    proprietarBazaDatesViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (action == ActionState.Edit)
            {
                dynamic selectedOrder = bazaDatesDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.idBaza;
                    var editedBaza = ctx.BazaDates.FirstOrDefault(s => s.idBaza == curr_id);
                    if (editedBaza != null)
                    {
                        editedBaza.idBaza = Int32.Parse(cmbProprietar.SelectedValue.ToString());
                        editedBaza.idAuto = Convert.ToInt32(cmbIntrari.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                proprietarViewSource.View.MoveCurrentTo(selectedOrder);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = bazaDatesDataGrid.SelectedItem;
                    int curr_id = selectedOrder.idBaza;
                    var deletedBaza = ctx.BazaDates.FirstOrDefault(s => s.idBaza == curr_id);
                    if (deletedBaza != null)
                    {
                        ctx.BazaDates.Remove(deletedBaza);
                        ctx.SaveChanges();
                        MessageBox.Show("Order Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BindDataGrid()
        {
            var queryBaza = from bd in ctx.BazaDates
                             join prop in ctx.Proprietars on bd.idProprietar equals
                             prop.idProprietar
                             join intr in ctx.Intraris on bd.idAuto equals intr.idAuto
                             select new
                             {
                                 bd.idBaza,
                                 bd.idAuto,
                                 bd.idProprietar,
                                 prop.Nume,
                                 prop.Prenume,
                                 intr.Model,
                                 intr.Marca
                             };
            proprietarBazaDatesViewSource.Source = queryBaza.ToList();
        }

        private void SetValidationBinding()
        {
            Binding numeValidationBinding = new Binding();
            numeValidationBinding.Source = proprietarViewSource;
            numeValidationBinding.Path = new PropertyPath("Nume");
            numeValidationBinding.NotifyOnValidationError = true;
            numeValidationBinding.Mode = BindingMode.TwoWay;
            numeValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            numeValidationBinding.ValidationRules.Add(new StringNotEmpty());
            numeTextBox.SetBinding(TextBox.TextProperty, numeValidationBinding);

            Binding prenumeValidationBinding = new Binding();
            prenumeValidationBinding.Source = proprietarViewSource;
            prenumeValidationBinding.Path = new PropertyPath("Prenume");
            prenumeValidationBinding.NotifyOnValidationError = true;
            prenumeValidationBinding.Mode = BindingMode.TwoWay;
            prenumeValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            prenumeValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            prenumeTextBox.SetBinding(TextBox.TextProperty, prenumeValidationBinding); //setare binding nou
        }

        private void btnPrevious0_Click(object sender, RoutedEventArgs e)
        {
            proprietarBazaDatesViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext0_Click(object sender, RoutedEventArgs e)
        {
            proprietarBazaDatesViewSource.View.MoveCurrentToNext();
        }

    }
}
