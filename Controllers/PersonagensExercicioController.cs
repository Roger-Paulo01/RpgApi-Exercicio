using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.Models.Enuns;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonagensExercicioController : ControllerBase
    {

        private static List<Personagem> pers = new List<Personagem>()
        {
            new Personagem() { Id = 1, Nome = "Frodo", PontosVida=100, Forca=17, Defesa=23, Inteligencia=33, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=100, Forca=15, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=100, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo},
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=100, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago},
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=100, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=100, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo},
            new Personagem() { Id = 7, Nome = "Ragadast", PontosVida=100, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago},
        };

        [HttpGet("GetByNome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Informe um nome para pesquisa.");

            var personagem = pers.FirstOrDefault(p =>
                p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (personagem is null)
            {
                return NotFound(new
                {
                    status = 404,
                    error = "Personagem não encontrado",
                    detalhe = $"Nenhum personagem com o nome '{nome}' foi encontrado."
                });
            }

            return Ok(personagem);
        }

        [HttpGet("GetClerigoMago")]
        public IActionResult GetClerigoMago()
        {
            var lista = pers.Where(l => l.Classe != ClasseEnum.Cavaleiro).OrderByDescending(l => l.PontosVida).ToList();
            return Ok(lista);
        }

        [HttpGet("GetEstatisticas")]
        public IActionResult GetEstatisticas()
        {
            var quantidade = pers.Count;
            var somma = pers.Sum(i => i.Inteligencia);
            var result = new { quantidade, somma };
            return Ok(result);
        }

        [HttpPost("PostValidacao")]
        public IActionResult PostValidacacao([FromBody] Personagem personagem)
        {
            if (personagem.Defesa < 10)
            {
                return BadRequest("Não é possivel adicionar personagem com defesa menor que 10");
            }

            if (personagem.Inteligencia > 30)
            {
                return BadRequest("Não é possivel adicionar personagem com inteligência maior que 30");
            }

            pers.Add(personagem);

            return Ok(pers);
        }

        [HttpPost("PostValidacaoMago")]
        public IActionResult PostValidacaoMago([FromBody] Personagem personagem)
        {
            personagem.Classe = ClasseEnum.Mago;
            if (personagem.Inteligencia < 35)
            {
                return BadRequest("Não é permitido magos com inteligência menor que 35");
            }
            pers.Add(personagem);

            return Ok(pers);
        }

        [HttpGet("GetByClasse/{classe}")]
        
            public IActionResult GetByClasse(int classe)
        {
            if (!Enum.IsDefined(typeof(ClasseEnum), classe))
            {
                return BadRequest("Clase informada não existente");
            }

                var id = (ClasseEnum)classe;
                var lista = pers.Where(l => l.Classe == id).ToList();
                
                return Ok(lista);
        }

    }
}