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
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepo;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;

        public FornecedoresController(IFornecedorRepository fornecedorReposittory,
                                      IMapper mapper,
                                      IFornecedorService fornecedorService,
                                      INotificador notificador,
                                      INotyfService notyf) : base(notificador)
        {
            _fornecedorRepo = fornecedorReposittory;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _notyf = notyf;
        }

        [Route("lista-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepo.ObterTodos()));
        }

        [Route("datalhes-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null) return NotFound();

            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            return View(fornecedorViewModel);
        }

        [Route("criar-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("criar-fornecedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return NotFound();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);

            if (!OperacaoValida()) return View(fornecedorViewModel);

            _notyf.Success("registro salvo com sucesso", 3);

            return RedirectToAction("Index");
        }

        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null) return NotFound();

            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            return View(fornecedorViewModel);
        }

        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Atualizar(fornecedor);

            if (!OperacaoValida()) return View(await ObterFornecedorProdutosEndereco(id));


            _notyf.Success("registro salvo com sucesso", 3);

            return RedirectToAction("Index");
        }

        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null) return NotFound();

            return View(fornecedorViewModel);
        }

        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null) return NotFound();

            await _fornecedorService.Remover(id);

            if (!OperacaoValida()) return View(fornecedor);

            _notyf.Success("fornecedor excluído com sucesso", 3);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null) return NotFound();

            return PartialView("_DetalhesEndereco", fornecedor);
        }

        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null) return NotFound();

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            if (!OperacaoValida()) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            _notyf.Success("registro salvo com sucesso", 3);

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.Id });

            return Json(new { success = true, url });
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepo.ObterFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepo.ObterFornecedorProdutosEndereco(id));
        }
    }
}
