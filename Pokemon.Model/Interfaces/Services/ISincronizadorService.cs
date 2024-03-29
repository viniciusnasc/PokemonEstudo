﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketMonster.Model.Interfaces.Services
{
    public interface ISincronizadorService
    {
        Task SincronizarPokemon();
        Task SincronizarTreinadores(string endereco);
        Task SincronizarGinasios(string endereco);
    }
}
