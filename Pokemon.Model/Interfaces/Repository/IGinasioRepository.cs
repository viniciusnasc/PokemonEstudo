﻿using PocketMonster.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketMonster.Model.Interfaces.Repository
{
    public interface IGinasioRepository
    {
        Task<bool> Incluir(Ginasio gym);
        Task<Ginasio> ProcurarPorNome(string nome);
        Task<bool> VerificarTreinadorLider(Treinador treinador);
        Task<bool> Alterar(Ginasio gym);
        Task<List<Ginasio>> SelecionarTudo();
        Task<Ginasio> SelecionarPorId(Guid id);
    }
}
