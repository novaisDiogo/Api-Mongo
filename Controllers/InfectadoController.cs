using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude, null);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizarInfectado(string id, [FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dataNascimento: dto.DataNascimento, sexo: dto.Sexo,
                latitude: dto.Latitude, longitude: dto.Longitude, id: id);

            var filter = Builders<Infectado>.Filter.Eq(s => s.Id, id);
            var result = _infectadosCollection.ReplaceOne(filter, infectado);

            //_infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(c => c.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("Sexo", dto.Sexo));

            return Ok("Atualizado com sucesso");
        }

        [HttpDelete("{id}")]
        public ActionResult DeletarInfectado(string id)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(c => c.Id == id));

            return Ok("Deletado com sucesso");
        }
    }
}
