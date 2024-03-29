﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketMonster.API.Extensoes;
using PocketMonster.Model.DTOs.InputModels;
using PocketMonster.Model.DTOs.OutputModels;
using PocketMonster.Model.Exceptions;
using PocketMonster.Model.Interfaces;
using PocketMonster.Model.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketMonster.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GinasioController : ControllerBase
    {
        private readonly IGinasioService _ginasioService;
        public readonly IUser UserApp;

        protected Guid UsuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }

        public GinasioController(IGinasioService ginasioService, IUser userApp)
        {
            _ginasioService = ginasioService;
            UserApp = userApp;

            if (UserApp.IsAuthenticated())
            {
                UsuarioId = userApp.GetUserId();
                UsuarioAutenticado = true;
            }
        }

        /// <summary>
        /// Lista todos os ginasios cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListarTodosGinasios")]
        [ClaimsAuthorize("Ginasio", "listar")]
        public async Task<ActionResult<IEnumerable<GinasioViewModel>>> ListarGinasios()
        {
            var treinadores = await _ginasioService.ListarGinasios();

            if (treinadores.Count() == 0)
                return NoContent();

            return Ok(treinadores);
        }

        /// <summary>
        /// Mostra os dados de um ginasio a partir de seu ID
        /// </summary>
        /// <param name="id">Id do ginasio</param>
        /// <returns></returns>
        [ClaimsAuthorize("Ginasio", "leitura")]
        [HttpGet("ProcurarPorId")]
        public async Task<ActionResult<GinasioViewModel>> ProcurarPorId(Guid id)
        {
            try
            {
                var ginasio = await _ginasioService.BuscarGinasioPorId(id);
                return Ok(ginasio);
            }
            catch (GinasioNaoEncontradoException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Mostra os dados de um ginasio a partir de seu nome
        /// </summary>
        /// <param name="nome">nome do ginasio</param>
        /// <returns></returns>
        [HttpGet("ProcurarPorNome")]
        [ClaimsAuthorize("Ginasio", "leitura")]
        public async Task<ActionResult<GinasioViewModel>> ProcurarPorNome(string nome)
        {
            try
            {
                var ginasio = await _ginasioService.BuscarGinasioPorNome(nome);
                return Ok(ginasio);
            }
            catch (GinasioNaoEncontradoException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Cria um novo ginasio. O treinador que será designado deve existir e ter 6 pokemon do tipo do ginasio
        /// </summary>
        /// <param name="dto">dados do ginasio, utilizar ingles no tipo</param>
        /// <returns></returns>
        [HttpPost("CriarNovoGinasio")]
        [ClaimsAuthorize("Ginasio", "adicionar")]
        public async Task<ActionResult<GinasioViewModel>> CadastrarGinasio([FromQuery]GinasioInputModel dto)
        {
            try
            {
                var ginasio = await _ginasioService.CriarGinasio(dto);
                return Ok(ginasio);
            }
            catch (GinasioJaExisteException e)
            {
                return BadRequest(e.Message);
            }
            catch (TreinadorNaoExisteException e)
            {
                return BadRequest(e.Message);
            }
            catch (TreinadorJaELiderException e)
            {
                return BadRequest(e.Message);
            }
            catch (TreinadorNaoPodeSerLiderException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Altera o lider do ginasio. O treinador que será designado deve existir e ter 6 pokemon do tipo do ginasio
        /// </summary>
        /// <param name="nome">nome do novo lider de ginasio</param>
        /// <param name="id">id do ginasio</param>
        /// <returns></returns>
        [HttpPut("AlterarLider")]
        public async Task<ActionResult<GinasioViewModel>> CadastrarGinasio([FromQuery] Guid id, string nome)
        {
            try
            {
                var ginasio = await _ginasioService.AlterarLider(id,nome);
                return Ok(ginasio);
            }
            catch (GinasioNaoEncontradoException e)
            {
                return BadRequest(e.Message);
            }
            catch (TreinadorNaoExisteException e)
            {
                return BadRequest(e.Message);
            }
            catch (TreinadorJaELiderException e)
            {
                return BadRequest(e.Message);
            }
            catch (TreinadorNaoPodeSerLiderException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
