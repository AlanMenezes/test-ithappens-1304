﻿namespace Models
{
    public class CriarProdutoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string CodBarra { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
    }
}