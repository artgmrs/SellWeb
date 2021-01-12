using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellWeb.App.ViewModels;
using SellWeb.Business.Interfaces;
using SellWeb.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SellWeb.App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepo;
        private readonly IFornecedorRepository _fornecedorRepo;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;

        public ProdutosController(IProdutoRepository produtoRepo,
                                  IMapper mapper,
                                  IFornecedorRepository fornecedorRepo,
                                  IProdutoService produtoService,
                                  INotificador notificador,
                                  INotyfService notyf) : base(notificador)
        {
            _produtoRepo = produtoRepo;
            _mapper = mapper;
            _fornecedorRepo = fornecedorRepo;
            _produtoService = produtoService;
            _notyf = notyf;
        }

        [Route("lista-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepo.ObterProdutosFornecedores()));
        }

        [Route("detalhes-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null) return NotFound();

            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [Route("criar-produto")]
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [Route("criar-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid) return View(produtoViewModel);

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            if (!OperacaoValida()) return View(produtoViewModel);

            _notyf.Success("registro salvo com sucesso", 3);

            return RedirectToAction("Index");
        }

        [Route("editar-produto")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [Route("editar-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            var produtoAtualizacao = await ObterProduto(id);
            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;

            if (!ModelState.IsValid) return View(produtoViewModel);

            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            if (!OperacaoValida()) return View(produtoViewModel);

            _notyf.Success("registro salvo com sucesso", 3);

            return RedirectToAction("Index");
        }

        [Route("excluir-produto")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null) return NotFound();

            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [Route("excluir-produto")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await ObterProduto(id);
            if (produto == null) return NotFound();

            await _produtoService.Remover(id);

            if (!OperacaoValida()) return View(produto);

            _notyf.Success("produto excluído com sucesso", 3);

            return RedirectToAction("Index");
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepo.ObterProdutoFornecedor(id));
            await PopularFornecedores(produto);
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepo.ObterTodos());
            return produto;
        }
    }
}
