using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {

        /// <summary>
        /// Consulta o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public List<DML.Beneficiario> Consultar(long idCliente)
        {
            DAL.DaoBeneficiario be = new DAL.DaoBeneficiario();
            return be.Consultar(idCliente);
        }


        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Incluir(beneficiario);
        }

        /// <summary>
        /// Exclui todos beneficiarios do cliente
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        public void ExcluirTodos(long idCliente)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            bene.ExcluirTodos(idCliente);
        }
    }
}
