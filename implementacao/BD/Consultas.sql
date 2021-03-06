/*** Buscando produtos com quantidade maior ou igual a 100***/
SELECT P.Descricao        AS Produto
      ,P.CodBarra 
      ,SUM (E.Quantidade) AS Quantidade
FROM Produto AS P
INNER JOIN Estoque AS E
   ON (P.Id = E.IdProduto)
GROUP BY P.Descricao
        ,P.CodBarra 
HAVING SUM (E.Quantidade) >= 100



/***Buscando produtos com estoque da Filial 60***/
SELECT P.Descricao  AS Produto
      ,E.Quantidade
FROM Produto AS P
INNER JOIN Estoque AS E
   ON (P.Id = E.IdProduto)
WHERE E.Quantidade > 0
AND E.CodFilial = 60



/***Buscando todos os campos do PedidoEstoque e ItensPedido filtrado pelo produto de código 7993***/
SELECT *
FROM PedidoEstoque AS P
INNER JOIN ItemPedido AS I
   ON (P.Id = I.Pedido)
WHERE I.Produto = 7993


/***Buscando todos os pedidos com suas respectivas formas de pagamento***/
SELECT P.*
      ,CASE
         WHEN V.FormaDePagamento = 1 THEN 
            'Avista'
         WHEN V.FormaDePagamento = 2 THEN 
            'Boleto'
         WHEN V.FormaDePagamento = 3 THEN 
            'Cartão'
       END               AS FormaDePagamento
FROM PedidoEstoque AS P
INNER JOIN Venda AS V
   ON (P.Id = V.PedidoEstoque)

/***Comparando se a soma dos valores dos itens de pedido bate com o valor total da capa do pedido***/
SELECT I.Pedido
      ,P.Valor           AS TotalCapa
      ,SUM(I.ValorTotal) AS TotalItens
      ,CASE
         WHEN P.Valor = SUM(I.ValorTotal) THEN 
            'Sim'
         ELSE
            'Não'
       END               AS Bateu
FROM PedidoEstoque AS P
INNER JOIN ItemPedido AS I
   ON (P.Id = I.Pedido)
GROUP BY I.Pedido
        ,P.Valor



/***Buscando todos os pedidos com mais de 10 itens***/
SELECT I.Pedido
      ,COUNT(*) AS TotalDeItens
FROM ItemPedido AS I
GROUP BY I.Pedido
HAVING COUNT(*) > 10