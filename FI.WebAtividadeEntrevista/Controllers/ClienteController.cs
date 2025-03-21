﻿using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Text.RegularExpressions;
using WebAtividadeEntrevista.Helper;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private static List<Beneficiario> beneficiarios = new List<Beneficiario>();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            beneficiarios.RemoveAll(c => true);
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario bene = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {

                if (bo.VerificarExistencia(model.Cpf))
                {
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado");
                }


                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    Cpf = model.Cpf
                });

                foreach (var beneficiario in beneficiarios)
                {
                    bene.Incluir(new Beneficiario()
                    {
                        Cpf = beneficiario.Cpf,
                        Nome = beneficiario.Nome,
                        IdCliente = model.Id
                    });
                }


                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario bene = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                Cliente ClienteAntigo = bo.Consultar(model.Id);
                var CpfNovo = StringFunctions.SoNumeroString(model.Cpf);

                if (ClienteAntigo.Cpf != CpfNovo && bo.VerificarExistencia(model.Cpf))
                {
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado");
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    Cpf = model.Cpf
                });

                bene.ExcluirTodos(model.Id);

                foreach (var beneficiario in beneficiarios)
                {
                    bene.Incluir(new Beneficiario()
                    {
                        Cpf = beneficiario.Cpf,
                        Nome = beneficiario.Nome,
                        IdCliente = model.Id
                    });
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario bene = new BoBeneficiario();
            Cliente cliente = bo.Consultar(id);

            beneficiarios.RemoveAll(c => true);
            beneficiarios = bene.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Cpf = cliente.Cpf
                };

            }

            Models.ClienteBeneficiarioModel modelClienteBeneficiario = new ClienteBeneficiarioModel()
            {
                Cliente = model,
                Beneficiarios = beneficiarios
            };

            return View(modelClienteBeneficiario);
        }

        [HttpPost]
        public JsonResult BeneficiarioIncluir(BeneficiarioModel model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    List<string> erros = (from item in ModelState.Values
                                          from error in item.Errors
                                          select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }
                else
                {

                    Beneficiario beneficiarioNovo = new Beneficiario()
                    {
                        Nome = model.Nome,
                        Cpf = model.Cpf
                    };

                    if (model.ExisteCpf(model.Cpf, beneficiarios))
                    {
                        Response.StatusCode = 400;
                        return Json("CPF já cadastrado nos beneficiários");
                    }

                    beneficiarios.Add(beneficiarioNovo);

                    return Json(new
                    {
                        Result = "OK",
                        Data = beneficiarios
                    });
                }
                ;
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult BeneficiarioAlterar(BeneficiarioModel model, int index)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    List<string> erros = (from item in ModelState.Values
                                          from error in item.Errors
                                          select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }
                else
                {
                    var cpfNum = StringFunctions.SoNumeroString(model.Cpf);
                    if (cpfNum != StringFunctions.SoNumeroString(beneficiarios[index].Cpf) && model.ExisteCpf(model.Cpf, beneficiarios))
                    {
                        Response.StatusCode = 400;
                        return Json("CPF já cadastrado nos beneficiários");
                    }

                    beneficiarios[index].Nome = model.Nome;
                    beneficiarios[index].Cpf = model.Cpf;

                    return Json(new
                    {
                        Result = "OK",
                        Data = beneficiarios
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult BeneficiarioExcluir(int Id)
        {
            try
            {

                beneficiarios.RemoveAt(Id);

                return Json(new
                {
                    Result = "OK",
                    Data = beneficiarios
                });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }

}