using DBContactLibraryFrameWork;
using DBContactLibraryFrameWork.Models;
using DBContactLibraryFrameWork.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBContactForms
{
    public partial class MyForm : Form
    {
        BindingSource contactListBindingSource = new BindingSource();
        private bool contactClicked = true;

        public MyForm()
        {
            InitializeComponent();
         
        }

        public List<Contact> GetContactsList()
        {
            using (ContactsService service = new ContactsService()) 
            { 
                return service.ReadAllContacts();
            }
        }

        public List<Address> GetAddressesList()
        {
            using (AddressesService service = new AddressesService())
            {
                return service.ReadAllAddresses();
            }
        }


        public void DataBinding()
        {
            contactListBindingSource.DataSource = GetContactsList();
            contactListBox.DataSource = contactListBindingSource;
            contactListBox.DisplayMember = "Display";
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var selected = contactListBox.SelectedItem;

            if (contactClicked)
            {
                using (ContactsService service = new ContactsService())
                {
                    int result = service.CreateContact(ssnTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, emailTextBox.Text);
                    
                    if (result == 0)
                    {
                        MessageBox.Show("Contact already exists");
                    }
                    contactListBindingSource.DataSource = GetContactsList();
                }
            } 
            else if (!contactClicked)
            {
                using (AddressesService service = new AddressesService())
                {
                    int zip;
                    int result;
                    try
                    {
                        zip = int.Parse(lastNameTextBox.Text.Replace(" ", ""));
                        result = service.CreateAddress(ssnTextBox.Text, firstNameTextBox.Text, zip);
                        if (result == 0)
                        {
                            MessageBox.Show("Contact already exists");
                        }
                        contactListBindingSource.DataSource = GetAddressesList();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Invalid ZIP");
                    } 
                }
            } 
            ClearTextFields(); 
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            var postToUpdate = contactListBox.SelectedItem;
            if (contactClicked)
            {
                using (ContactsService service = new ContactsService())
                {
                    var contactToUpdate = (Contact)postToUpdate;
                    service.UpdateContact("UpdateContact", contactToUpdate.Id, ssnTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, emailTextBox.Text);
                    contactListBindingSource.DataSource = GetContactsList();
                }
            }
            else if (!contactClicked)
            {
                using (AddressesService service = new AddressesService())
                {
                    var addressToUpdate = (Address)postToUpdate;
                    int zip;
                  
                    try
                    {
                        zip = int.Parse(lastNameTextBox.Text.Replace(" ", ""));
                        service.UpdateAddress("UpdateAddress", addressToUpdate.Id, ssnTextBox.Text, firstNameTextBox.Text, zip);
                        contactListBindingSource.DataSource = GetAddressesList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

            var postToDelete = contactListBox.SelectedItem;

            if (contactClicked)
            {
                Contact contactToDelete = (Contact)postToDelete;
                using (ContactsService service = new ContactsService())
                {
                    service.DeleteContact("DeleteContact", contactToDelete.Id);
                    contactListBindingSource.DataSource = GetContactsList();
                    contactListBox.DataSource = contactListBindingSource;
                }
            } 
            else if (!contactClicked)
            {
                Address addressToDelete = (Address)postToDelete;
                using (AddressesService service = new AddressesService())
                {
                    service.DeleteContact("DeleteAddress", addressToDelete.Id);
                    contactListBindingSource.DataSource = GetAddressesList();
                    contactListBox.DataSource = contactListBindingSource;
                }
            }
          
            ClearTextFields();
        }

        private void ClearTextFields()
        {
            ssnTextBox.Text = "";
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
        }

        private void contactListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelected();
        }

        private void showContactsButton_Click(object sender, EventArgs e)
        {
            contactListBindingSource.DataSource = GetContactsList();
            contactListBox.DataSource = contactListBindingSource;

            emailLabel.Visible = true;
            emailTextBox.Visible = true;
            ssnLabel.Text = "SSN";
            firstNameLabel.Text = "First Name";
            lastNameLabel.Text = "Last Name";
            emailLabel.Text = "E-Mail";
            contactClicked = true;
            CheckSelected();
        }

     

        private void showAddressButtons_Click(object sender, EventArgs e)
        {
            //contactListBindingSource.DataSource = GetAddressesList();
            //contactListBox.DataSource = contactListBindingSource;

            ssnLabel.Text = "City";
            firstNameLabel.Text = "Street";
            lastNameLabel.Text = "Zip";
            emailLabel.Visible = false;
            emailTextBox.Visible = false;


            var selected = (Contact)contactListBox.SelectedItem;
            using (C2AService service = new C2AService())
            {
                var selectedContactsContactInfo = service.ReadContactsAddress(selected.Id);

                if (selectedContactsContactInfo != null)
                {
                    ssnTextBox.Text = selectedContactsContactInfo.City;
                    firstNameTextBox.Text = selectedContactsContactInfo.Street;
                    lastNameTextBox.Text = selectedContactsContactInfo.Zip.ToString();
                }
                    
     
            }
            contactClicked = false;

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearTextFields();
        }

        private void CheckSelected()
        {
            var selected = (Contact)contactListBox.SelectedItem;
            if (selected != null)
            {
                if (contactClicked)
                {
                    var contact = (Contact)selected;
                    ssnTextBox.Text = contact.SSN;
                    firstNameTextBox.Text = contact.FirstName;
                    lastNameTextBox.Text = contact.LastName;
                    emailTextBox.Text = contact.Email;
                }
                else if (!contactClicked)
                {
                   
                    using (C2AService service = new C2AService())
                    {
                        var selectedContactsContactInfo = service.ReadContactsAddress(selected.Id);

                        if (selectedContactsContactInfo != null)
                        {
                            ssnTextBox.Text = selectedContactsContactInfo.City;
                            firstNameTextBox.Text = selectedContactsContactInfo.Street;
                            lastNameTextBox.Text = selectedContactsContactInfo.Zip.ToString();
                        } 
                        else
                        {
                            MessageBox.Show("Kontakt saknar adress");
                            ClearTextFields();
                        }
                        

                    }
                    contactClicked = false;
                }

            }
        }

        private void addTestButton_Click(object sender, EventArgs e)
        {
            var selected = contactListBox.SelectedItem;
            var testa = sender.GetType();
            var test = selected.GetType();
          //  service.Create(selected,ssnTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, emailTextBox.Text);
        }

        private void ContactInformation_Click(object sender, EventArgs e)
        {

        }
    }
}
