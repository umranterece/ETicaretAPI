using System.Data;
using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products;

public class CreateProductValidator:AbstractValidator<VM_Create_Product>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Lutfen urun adini bos gecmeyin")
            .MaximumLength(150)
            .MinimumLength(5)
            .WithMessage("Lutfen urun adini 5 ile 150 karakter arasinda giriniz.");
        
        RuleFor(p=>p.Stock)
            .NotEmpty()
            .NotNull()
            .WithMessage("Lutfen stok bilgisini giriniz")
            .Must(s=>s>=0)
            .WithMessage("Stok bilgisi sifirdan kucuk olmamalidir.");
        
        RuleFor(p=>p.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Lutfen fiyat bilgisini giriniz")
            .Must(s=>s>=0)
            .WithMessage("Fiyat bilgisi sifirdan kucuk olmamalidir.");
    }
}