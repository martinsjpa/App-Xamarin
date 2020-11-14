﻿using Desafio.Models;
using Desafio.Repository;
using Desafio.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Desafio
{
    public partial class MainPage : ContentPage
    {
        public List<AddressRepository> addressRepositories { get; set; }

        public MainPage()
        {
            InitializeComponent();
            GetAddressBaseAsync();
            BUTTON.Clicked += FindCep;

        }

        private void FindCep(object sender, EventArgs args)
        {
            var find = CEP.Text.Trim();
            find = find.Replace("-", "");
            if(isValidCep(find))
            {
                try
                {
                    Address result = ViaCepService.FindAdressViaCep(find);

                    if(!addressRepositories.Contains((object)result))
                    {
                        var test = App.Database.SaveAddressAsync(new AddressRepository(result.Cep, result.Logradouro, result.Bairro, result.Logradouro, result.Uf));
                        GetAddressBaseAsync();
                    }
                    
                   
                    RESULT.Text = string.Format("Endereço: {0}, {1}, {2}, {3}, {4}", result.Cep, result.Logradouro, result.Bairro, result.Localidade, result.Uf);
                }
                catch(Exception e)
                {
                    DisplayAlert("Erro:", e.Message, "OK");
                }
                
            }
            
        }

        private bool isValidCep(string cep)
        {
            if(cep.Length != 8)
            {
                DisplayAlert("Erro: CEP Inválido", "O CEP deve precisa ter 8 caractéres", "OK");
                return false;
            }
            int checkCep = 0;
            if(!int.TryParse(cep,out checkCep))
            {
                DisplayAlert("Erro: CEP Inválido", "O CEP deve ser composto somente por numeros", "OK");
                return false;
            }

            return true;
        }

        private async void GetAddressBaseAsync()
        {
            addressRepositories = await App.Database.GetAddressAsync();
            listView.ItemsSource = addressRepositories; 
        }
    }
}
