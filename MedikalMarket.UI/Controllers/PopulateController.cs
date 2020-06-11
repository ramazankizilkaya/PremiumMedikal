using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MedikalMarket.UI.Controllers
{
    public class PopulateController : Controller
    {
        private readonly IAdProductRepository _adproRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITopCategoryRepository _topCateRepo;
        private readonly IMiddleCategoryRepository _midCateRepo;
        private readonly ISubCategoryRepository _subCateRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductPhotoRepository _proPhotoRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly ISliderRepository _sliderRepo;
        private readonly IMiniSliderRepository _miniSliderRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IAdminRepository _adminRepo;
        private readonly IFavoriteProductRepository _fpRepo;
        private readonly IWebHostEnvironment _environment;


        public PopulateController(ITopCategoryRepository topCateRepo, IMiddleCategoryRepository midCateRepo, ISubCategoryRepository subCateRepo, IProductRepository productRepo, IBrandRepository brandRepo, ISliderRepository sliderRepo, IWebHostEnvironment hostingEnvironment, IMiniSliderRepository miniSliderRepo, IProductPhotoRepository proPhotoRepo, IAdProductRepository adproRepo, ICustomerRepository customerRepo, IAdminRepository adminRepo, IFavoriteProductRepository fpRepo, IWebHostEnvironment environment)
        {
            _topCateRepo = topCateRepo;
            _midCateRepo = midCateRepo;
            _subCateRepo = subCateRepo;
            _productRepo = productRepo;
            _brandRepo = brandRepo;
            _sliderRepo = sliderRepo;
            _hostingEnvironment = hostingEnvironment;
            _miniSliderRepo = miniSliderRepo;
            _proPhotoRepo = proPhotoRepo;
            _adproRepo = adproRepo;
            _customerRepo = customerRepo;
            _adminRepo = adminRepo;
            _fpRepo = fpRepo;
            _environment = environment;
        }

        public IActionResult Index()
        {
            PopluateTopCategories();
            PopluateMiddleCategories();
            PopluateSubCategories();
            PopulateBrands();
            PopulateProducts();
            PopulateSliders();
            PopulateMiniSliders();
            PopulateProductMainPhotos();
            PopulateProductAdditionalPhotos();
            PopulateAdProducts();
            PopulateCustomers();
            PopulateFavoriteProducts();
            PopulateAdmins();

            PopulateInfoDto dto = new PopulateInfoDto();
            dto.Counts.Add("Top Kategori Sayısı", _topCateRepo.GetAllEntities().Count());
            dto.Counts.Add("Middle Kategori Sayısı", _midCateRepo.GetAllEntities().Count());
            dto.Counts.Add("Sub Kategori Sayısı", _subCateRepo.GetAllEntities().Count());

            dto.Counts.Add("Türkçe Slider Sayısı", _sliderRepo.FindEntities(x => x.Culture.Equals("tr")).Count());
            dto.Counts.Add("İngilizce Slider Sayısı", _sliderRepo.FindEntities(x => x.Culture.Equals("en")).Count());
            dto.Counts.Add("Rusça Slider Sayısı", _sliderRepo.FindEntities(x => x.Culture.Equals("ru")).Count());

            dto.Counts.Add("Türkçe MiniSlider Sayısı", _miniSliderRepo.FindEntities(x => x.Culture.Equals("tr")).Count());
            dto.Counts.Add("İngilizce MiniSlider Sayısı", _miniSliderRepo.FindEntities(x => x.Culture.Equals("en")).Count());
            dto.Counts.Add("Rusça MiniSlider Sayısı", _miniSliderRepo.FindEntities(x => x.Culture.Equals("ru")).Count());

            dto.Counts.Add("Marka Sayısı", _brandRepo.GetAllEntities().Count());
            dto.Counts.Add("Ürün Sayısı", _productRepo.GetAllEntities().Count());

            dto.Counts.Add("Ana Fotoğraf Sayısı", _proPhotoRepo.FindEntities(x => x.IsMainPhoto).Count());
            dto.Counts.Add("İlave Fotoğraf Sayısı", _proPhotoRepo.FindEntities(x => !x.IsMainPhoto).Count());

            dto.Counts.Add("Reklam Ürünü Sayısı", _adproRepo.GetAllEntities().Count());

            dto.Counts.Add("Müşteri Sayısı", _customerRepo.GetAllEntities().Count());
            dto.Counts.Add("Admin Sayısı", _adminRepo.GetAllEntities().Count());
            dto.Counts.Add("Favori Ürün Sayısı", _fpRepo.GetAllEntities().Count());

            return View(dto);
        }


        public void PopluateTopCategories()
        {
            string[] topCatesTR = { "Anatomik Modeller", "Anne ve Bebek Sağlığı", "Cihazlar", "Hasta Bakım Ürünleri", "Hasta Mobilyaları", "Kişisel Bakım ve Sağlık", "Ortopedi Ürünleri", "OSGB ve İSGB Malzemeleri", "Sarf Malzemeleri", "Veteriner Malzemeleri" };

            string[] topCatesEN = { "Anatomical Models", "Mother and Baby Health", "Devices", "Patient Care Products", "Patient Furniture", "Personal Care and Health", "Orthopedic Products", "OSGB and ISGB Materials", "Consumables", "Veterinary Supplies" };

            string[] topCatesRU = { "Анатомические Модели", "Здоровье матери и ребенка", "Устройства", "Продукты Для Ухода За Пациентами", "Мебель Для Пациентов", "Личная гигиена и здоровье", "Ортопедические Изделия", "Материалы OSGB и ISGB", "Расходные Материалы", "Ветеринарные Принадлежности" };

            if (!_topCateRepo.GetAllEntities().Any())
            {
                for (int i = 0; i < topCatesTR.Length; i++)
                {
                    _topCateRepo.CreateEntity(new TopCategory
                    {
                        NameTR = topCatesTR[i],
                        NameEN = topCatesEN[i],
                        NameRU = topCatesRU[i],
                        HasMiddleCategories = i == 7 ? false : i == 9 ? false : true,
                        TopCategoryNameUrlTR = topCatesTR[i].ConvertToFriendlyUrl(),
                        TopCategoryNameUrlEN = topCatesEN[i].ConvertToFriendlyUrl(),
                        TopCategoryNameUrlRU = topCatesEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.top,
                        HeadDescriptionTR="Türkçe meta Description",
                        HeadDescriptionEN= "İngilizce meta Description",
                        HeadDescriptionRU= "Rusça meta Description",
                        HeadTitleTR= "Türkçe meta Title",
                        HeadTitleEN= "İngilizce meta Title",
                        HeadTitleRU= "Rusça meta Title"
                    });
                }
            }

        }

        public void PopluateMiddleCategories()
        {
            string[] anatomikModellerTR = { "Anatomik Modeller", "Eğitim Maketleri", "İskelet Modelleri", "Kafatası Modelleri", "Tablolar", "Torso ve Kas Modelleri" };

            string[] anatomikModellerEN = { "Anatomical Models", "Education Models", "Skeletal Models", "Skull Models", "Tables", "Torso and Muscle Models" };

            string[] anatomikModellerRU = { "Анатомические Модели", "Учебные Макеты", "Модели Скелета", "Модели Черепа", "Таблицы", "Модели туловища и мышц" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 1).Any())
            {
                for (int i = 0; i < anatomikModellerTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = anatomikModellerTR[i],
                        NameEN = anatomikModellerEN[i],
                        NameRU = anatomikModellerRU[i],
                        MiddleCategoryNameUrlTR = anatomikModellerTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = anatomikModellerEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = anatomikModellerEN[i].ConvertToFriendlyUrl(),
                        TopCategoryId = 1,
                        HasSubCategories = false,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }



            string[] anneVeBebekSağlığıTR = { "Anne Sağlığı", "Bebek Bezleri", "Bebek Sağlığı", "Bebek Tartıları", "Biberonlar ve Emzikler", "Burun Aspiratör Cihazları", "Göğüs Koruyucular ve Kremler", "Göğüs Süt Pompaları", "Kamera ve Telsiz", "Süt Saklama Poşetleri" };

            string[] anneVeBebekSağlığıEN = { "Maternal Health", "Baby Diapers", "Baby Health", "Baby Scales", "Baby Bottles and Pacifiers", "Nasal Aspirator Devices", "Breast Protectors and Creams", "Breast Milk Pumps", "Camera and Radio", "Milk Storage Bags" };

            string[] anneVeBebekSağlığıRU = { "Материнское Здоровье", "Подгузники", "Здоровье Ребенка", "Детские Весы", "Бутылочки и пустышки", "Носовые Аспираторные Устройства", "Грудные консерванты и кремы", "Грудные Молочные Насосы", "Камера и рация", "Мешки Для Хранения Молока" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 2).Any())
            {
                for (int i = 0; i < anneVeBebekSağlığıTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = anneVeBebekSağlığıTR[i],
                        NameEN = anneVeBebekSağlığıEN[i],
                        NameRU = anneVeBebekSağlığıRU[i],
                        MiddleCategoryNameUrlTR = anneVeBebekSağlığıTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = anneVeBebekSağlığıEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = anneVeBebekSağlığıEN[i].ConvertToFriendlyUrl(),
                        TopCategoryId = 2,
                        HasSubCategories = false,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] cihazlarTR = { "Adım Sayar ve Nabız Ölçer", "Ateş Ölçerler/Termometreler", "Boy ve Kilo Ölçerler", "Cihaz Pilleri", "Masaj Aleti ve Tens Cihazları", "Nebulizatörler", "Oksijen Tüpü ve Manometre", "Solunum Destek Cihazları", "Steteskoplar", "Tansiyon Aletleri", "Tıbbi Cihazlar" };

            string[] cihazlarEN = { "Pedometer and Pulse Meter", "Fever Meters/Thermometers", "Height and Weight Meters", "Device Batteries", "Massage Tools and Tens Devices", "Nebulizers", "Oxygen Tube and Manometer", "Respiratory Support Devices", "Stethoscopes", "Sphygmomanometers", "Medical Devices" };

            string[] cihazlarRU = { "Шагомер и пульсометр", "Лихорадка/термометры", "Метры роста и веса", "Аккумуляторы для устройств", "Массажные инструменты и десятки устройств", "Распылители", "Кислородная трубка и манометр", "Устройства респираторной поддержки", "стетоскопы", "Сфигмоманометры", "Медицинские приборы" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 3).Any())
            {
                for (int i = 0; i < cihazlarTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = cihazlarTR[i],
                        NameEN = cihazlarEN[i],
                        NameRU = cihazlarRU[i],
                        TopCategoryId = 3,
                        MiddleCategoryNameUrlTR = cihazlarTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = cihazlarEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = cihazlarEN[i].ConvertToFriendlyUrl(),
                        HasSubCategories = i == 0 ? false : i == 3 ? false : i == 5 ? false : i == 7 ? false : i == 8 ? false : true,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] hastaBakimUrunleriTR = { "Ağız Temizliği", "Çeşitli Ürünler", "Diyabet Ürünleri", "Eldiven ve Maskeler", "Göğüs Alerjisi Ürünleri", "Göz Bantları", "Hasta Bezleri", "Hasta Yatak-Karyola", "Hasta Yıkama Ürünleri", "Havalı Yatak Sistemleri", "Kanül Sabitleyiciler ve Pedler", "Klozetler ve Yükselticiler", "Mendil, Havlu, Saç Bonesi", "Ördek-Sürgü", "Solunum Destek Ürünleri", "Tekerlekli Sandalyeler", "Tıbbi Tekstil Ürünleri", "Yara Bakım Ürünleri", "Yardımcı Ürünler" };

            string[] hastaBakimUrunleriEN = { "Oral Cleaning", "Miscellaneous Products", "Diabetes Products", "Gloves and Masks", "Chest Allergy Products", "Eye Tapes", "Patient Nappies", "Patient Bed-Bedstead", "Patient Washing Products", "Air Bed Systems", "Cannula Stabilizers and Pads", "Toilets and Risers", "Handkerchief, Towel, Hair Cap", "Duck-Slider", "Respiratory Support Products", "Wheelchairs", "Medical Textile Products", "Wound Care Products", "Auxiliary Products" };

            string[] hastaBakimUrunleriRU = { "Оральная чистка", "Разные продукты", "Продукты диабета", "Перчатки и маски", "Грудь Продукты Аллергии", "Глазные ленты", "Подгузники для пациентов", "Кровать для пациента", "Моющие средства для пациентов", "Системы надувных кроватей", "Стабилизаторы канюли и колодки", "Туалеты и стояки", "Носовой платок, полотенце, шапочка для волос", "Утка-Роча", "Продукты респираторной поддержки", "Инвалидные", "Медицинские Текстильные Изделия", "Средства для ухода за раной", "Вспомогательные продукты" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 4).Any())
            {
                for (int i = 0; i < hastaBakimUrunleriTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = hastaBakimUrunleriTR[i],
                        NameEN = hastaBakimUrunleriEN[i],
                        NameRU = hastaBakimUrunleriRU[i],
                        MiddleCategoryNameUrlTR = hastaBakimUrunleriTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = hastaBakimUrunleriEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = hastaBakimUrunleriEN[i].ConvertToFriendlyUrl(),
                        TopCategoryId = 4,
                        HasSubCategories = i == 2 ? true : i == 4 ? true : i == 6 ? true : i == 14 ? true : i == 15 ? true : i == 16 ? false : i == 17 ? true : false,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }


            string[] hastaMobilyalarıTR = { "Cihaz Sehpaları", "Dolaplar", "Eskoba/Basamak", "Göz Eşeli", "İlaç ve Acil Arabaları", "Jinekoloji ve Kan Alma Koltuğu", "Karyola ve Yataklar", "Muayene ve Masaj Masası", "Negatoskop", "Pansuman Arabaları", "Paravanlar", "Sedyeler", "Serum Askısı ve Lamba", "Tabureler", "Tekerlekli Sandalye", "Yatak Sehpası", "Yemek Masası ve Etejer" };

            string[] hastaMobilyalarıEN = { "Device Stands", "Cabinets", "Step Ladder", "Eye Chart", "Drug Trolley", "Gynecology and Blood Collection Chair", "Bed-Bedstead", "Examination and Massage Table", "Negatoscop", "Dressing Trolleys", "Paravans", "Stretchers", "Serum Hanger and Lamp", "Stools", "Wheelchairs", "Bed Table", "Dining Table and Shelf" };

            string[] hastaMobilyalarıRU = { "Подставки Для Устройств", "Шкафы", "Ступенчатая Лестница", "Глазная Карта", "Тележка Для Наркотиков", "Гинекология и стул для взятия крови", "Кроватки и кровати", "Осмотр и массажный стол", "Негатоскоп", "Перевязочные Тележки", "Параваны", "Носилки", "Вешалка для сыворотки и лампа", "Табуреты", "qная коляска", "Подставка Для Кровати", "Обеденный стол и Этехер" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 5).Any())
            {
                for (int i = 0; i < hastaMobilyalarıTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = hastaMobilyalarıTR[i],
                        NameEN = hastaMobilyalarıEN[i],
                        NameRU = hastaMobilyalarıRU[i],
                        MiddleCategoryNameUrlTR = hastaMobilyalarıTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = hastaMobilyalarıEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = hastaMobilyalarıEN[i].ConvertToFriendlyUrl(),
                        TopCategoryId = 5,
                        HasSubCategories = false,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] kisiselBakimVeSaglikTR = { "Ağız ve Diş Bakımı", "Alerji Eldivenleri", "Ayak Sağlığı Ürünleri", "Burun Tıkaçları", "Çeşitli Ürünler", "Göğüs Protezi Ürünleri", "Horlama Aparatları", "İlaç Saklama Kutuları", "Kişisel Kremler", "Kulak Tıkaçları", "Masaj Taytları", "Varis Çorapları" };

            string[] kisiselBakimVeSaglikEN = { "Oral and Dental Care", "Allergy Gloves", "Foot Health Products", "Nose Stoppers", "Miscellaneous Products", "Breast Prosthesis Products", "Snoring Apparatus", "Drug Storage Boxes", "Personal Creams", "Ear Plugs", "Massage Tights", "Varicose Stockings" };

            string[] kisiselBakimVeSaglikRU = { "Уход за полостью рта и зубами", "Аллергия Перчатки", "Продукты для здоровья ног", "Стопы носа", "Разные продукты", "Продукты для протезирования молочной железы", "Храп Аппарат", "Ящики для хранения наркотиков", "Личные кремы", "Беруши", "Массажные колготки", "Варикозные чулки" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 6).Any())
            {
                for (int i = 0; i < kisiselBakimVeSaglikTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = kisiselBakimVeSaglikTR[i],
                        NameEN = kisiselBakimVeSaglikEN[i],
                        NameRU = kisiselBakimVeSaglikRU[i],
                        MiddleCategoryNameUrlTR = kisiselBakimVeSaglikTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = kisiselBakimVeSaglikEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = kisiselBakimVeSaglikEN[i].ConvertToFriendlyUrl(),
                        TopCategoryId = 6,
                        HasSubCategories = i == 2 ? true : i == 11 ? true : false,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] ortopediUrunleriTR = { "Alçı Ürünleri", "Ayak Ürünleri", "Ayakkabı ve Terlikler", "Boyunluklar", "Dirsek Ürünleri", "Diz ve Bacak Ürünleri", "El ve Bilek Ürünleri", "Fizik Tedavi Ürünleri", "Korseler", "Metal Ortopedik Ürünler", "Özel Kuşak ve Destekler", "Sporcu Sağlığı Ürünleri" };

            string[] ortopediUrunleriEN = { "Plaster Products", "Foot Products", "Shoes and Slippers", "lanyards", "Elbow Products", "Knee and Leg Products", "Hand and Wrist Products", "Physical Therapy Products", "Corsets", "Metal Orthopedic Products", "Special Belts and Supports", "Sportsman's Health Products" };

            string[] ortopediUrunleriRU = { "Гипсовые изделия", "Продукты для ног", "Обувь и тапочки", "стропы", "Продукты локтя", "Продукты колена и ноги", "Продукты для рук и запястий", "Продукты для рук и запястий", "корсеты", "Металлические ортопедические изделия", "Специальный пояс и опоры", "Товары для здоровья спортсмена" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 7).Any())
            {
                for (int i = 0; i < ortopediUrunleriTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = ortopediUrunleriTR[i],
                        NameEN = ortopediUrunleriEN[i],
                        NameRU = ortopediUrunleriRU[i],
                        MiddleCategoryNameUrlTR = ortopediUrunleriTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = ortopediUrunleriEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = ortopediUrunleriEN[i].ConvertToFriendlyUrl(),
                        TopCategoryId = 7,
                        HasSubCategories = i == 7 ? true : i == 11 ? true : false,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] sarfMalzemeleriTR = { "Aile Hekimliği Malzemeleri", "Aletler", "Anestezi", "Antiseptik ve Dezenfektanlar", "Beslenme Sarf Malzemeleri", "Biyopsi İğneleri", "Cerrahi Kep, Bone, Galoş", "Cerrahi Ürünler ve İplikler", "Dermatoloji Ürünleri", "Drenaj-Artroskopi Seti", "Enjektör-İğne-İnfüzyon", "Gazlı Sargı Bezleri, Bandajlar", "Göz Bantları", "İlk Yardım Ürünleri", "İndikatörler", "Kadın Doğum", "Kan Transfüzyon Seti", "Kanama Durdurucular", "KBB Ürünleri", "Kemoterapi Ürünleri", "Laboratuvar Ürünleri", "Muayene Masa Örtüleri", "Örtüler ve Diğer Ürünler", "Sondalar ve İdrar Torbaları", "Stapler", "Steril Ürünler ve Pamuklar", "Tıbbi Elastik Bandajlar", "Tıbbi Flasterler", "Tomografi Ürünleri", "Ultrason, EKG Jeli, Elektrot", "Üroloji Sarf Malzemeleri" };

            string[] sarfMalzemeleriEN = { "Family Practice Materials", "Apparatus", "Anesthesia", "Antiseptic and Disinfectants", "Nutritional Consumables", "Biopsy Needles", "Surgical Cap, Bone, Galoshes", "Surgical Products and Yarns", "Dermatology Products", "Drainage-Arthroscopy Set", "Syringe-needle-Infusion", "Gas Bandages, Bandages", "Eye Tapes", "First Aid Products", "Indicators", "Women's Birth", "Blood Transfusion Set", "Bleeding Stoppers", "ENT Products", "Chemotherapy Products", "Laboratory Products", "Inspection Table Cloths", "Drapes and Other Products", "Probes and Urine Bags", "Staples", "Sterile Products and Cottons", "Medical Elastic Bandages", "Medical Plasters", "Tomography Products", "Ultrasound, ECG Gel, Electrode", "Urology Consumables" };

            string[] sarfMalzemeleriRU = { "Материалы Для Семейной Практики", "Устройство", "Анестезия", "Антисептические и дезинфицирующие средства", "Расходные материалы", "Иглы Для Биопсии", "Хирургическая Шапочка, Кость, Калоши", "Хирургическая продукция и пряжа", "Продукты Для Дерматологии", "Дренаж-Артроскопический Набор", "Шприц-игла-инфузия", "Газовые Повязки, Бинты", "Глазные Ленты", "Продукты Первой Помощи", "Указатели", "Рождение женщины ", "Набор Для Переливания Крови", "Кровоточащие Пробки", "ЛОР-продукты", "Продукты Химиотерапии", "Лабораторные Продукты", "Скатерти для осмотра", "Драпировки и другие продукты", "Зонды и мешки для мочи", "Скобы", "Стерильные продукты и хлопок", "Медицинские эластичные бинты", "Лечебные пластыри", "Томографические продукты", "Ультразвук, ЭКГ Гель, Электрод", "Расходные материалы для урологии" };

            if (!_midCateRepo.FindEntities(x => x.TopCategoryId == 9).Any())
            {
                for (int i = 0; i < sarfMalzemeleriTR.Length; i++)
                {
                    _midCateRepo.CreateEntity(new MiddleCategory
                    {
                        NameTR = sarfMalzemeleriTR[i],
                        NameEN = sarfMalzemeleriEN[i],
                        NameRU = sarfMalzemeleriRU[i],
                        MiddleCategoryNameUrlTR = sarfMalzemeleriTR[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlEN = sarfMalzemeleriEN[i].ConvertToFriendlyUrl(),
                        MiddleCategoryNameUrlRU = sarfMalzemeleriEN[i].ConvertToFriendlyUrl(),
                        TopCategoryId = 9,
                        HasSubCategories = i == 0 ? true : i == 13 ? true : i == 18 ? true : false,
                        CategoryType = CategoryType.mid,
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }
        }

        public void PopluateSubCategories()
        {
            #region Cihazlar
            string[] atesOlcerlerTermometrelerTR = { "Ateş Ölçerler/Termometreler", "Isı Kaydedici Termometreler" };
            string[] atesOlcerlerTermometrelerEN = { "Fever Meters/Thermometers", "Heat Recorder Thermometers" };
            string[] atesOlcerlerTermometrelerRU = { "Лихорадка/термометры", "Термометр с регистратором тепла" };

            int atesOlcerlerTermometrelerId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Ateş Ölçerler/Termometreler")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == atesOlcerlerTermometrelerId).First().SubCategories.Any())
            {
                for (int i = 0; i < atesOlcerlerTermometrelerTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = atesOlcerlerTermometrelerTR[i],
                        NameEN = atesOlcerlerTermometrelerEN[i],
                        NameRU = atesOlcerlerTermometrelerRU[i],
                        MiddleCategoryId = atesOlcerlerTermometrelerId,
                        SubCategoryNameUrlTR = atesOlcerlerTermometrelerTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = atesOlcerlerTermometrelerEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = atesOlcerlerTermometrelerEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, 
                        HeadDescriptionTR = "Türkçe meta Description", 
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }



            string[] boyVeKiloOlcerlerTR = { "Boy Ölçerler", "Kilo Ölçerler", "Mutfak Tartıları" };
            string[] boyVeKiloOlcerlerEN = { "Height Measuring Devices", "Body Weight Scales", "Kitchen Scales" };
            string[] boyVeKiloOlcerlerRU = { "Приборы для измерения высоты", "Весы для тела", "Кухонные весы" };

            int boyVeKiloOlcerlerId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Boy ve Kilo Ölçerler")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == boyVeKiloOlcerlerId).First().SubCategories.Any())
            {
                for (int i = 0; i < boyVeKiloOlcerlerTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = boyVeKiloOlcerlerTR[i],
                        NameEN = boyVeKiloOlcerlerEN[i],
                        NameRU = boyVeKiloOlcerlerRU[i],
                        MiddleCategoryId = boyVeKiloOlcerlerId,
                        SubCategoryNameUrlTR = boyVeKiloOlcerlerTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = boyVeKiloOlcerlerEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = boyVeKiloOlcerlerEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] masajAletiVeTensCihazıTR = { "Ayak Masaj Aletleri", "Masaj Koltuğu", "Masaj Minderi/Şiltesi", "Tens Cihazları ve Aksesuarları", "Vücut Masaj Aletleri" };
            string[] masajAletiVeTensCihazıEN = { "Foot Massage Tools", "Massage Chairs", "Massage Cushion/Mattress", "Tens Devices and Accessories", "Body Massage Tools" };
            string[] masajAletiVeTensCihazıRU = { "Инструменты для массажа ног", "Массажное кресло", "Массажная подушка/матрас", "Десятки устройств и аксессуаров", "Инструменты для массажа тела" };

            int masajAletiVeTensCihazıId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Masaj Aleti ve Tens Cihazları")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == masajAletiVeTensCihazıId).First().SubCategories.Any())
            {
                for (int i = 0; i < masajAletiVeTensCihazıTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = masajAletiVeTensCihazıTR[i],
                        NameEN = masajAletiVeTensCihazıEN[i],
                        NameRU = masajAletiVeTensCihazıRU[i],
                        MiddleCategoryId = masajAletiVeTensCihazıId,
                        SubCategoryNameUrlTR = masajAletiVeTensCihazıTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = masajAletiVeTensCihazıEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = masajAletiVeTensCihazıEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }


            string[] oksijenTupuVeManometreTR = { "Manometreler", "Oksijen Tüpü", "Tüp Taşıma Arabası/Çantası" };
            string[] oksijenTupuVeManometreEN = { "Pressure gauges", "Oxygen Tube", "Tube Transport Trolley/Bag" };
            string[] oksijenTupuVeManometreRU = { "манометры", "Кислородная трубка", "Тележка для перевозки труб/сумка" };

            int oksijenTupuVeManometreId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Oksijen Tüpü ve Manometre")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == oksijenTupuVeManometreId).First().SubCategories.Any())
            {
                for (int i = 0; i < oksijenTupuVeManometreTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = oksijenTupuVeManometreTR[i],
                        NameEN = oksijenTupuVeManometreEN[i],
                        NameRU = oksijenTupuVeManometreRU[i],
                        MiddleCategoryId = oksijenTupuVeManometreId,
                        SubCategoryNameUrlTR = oksijenTupuVeManometreTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = oksijenTupuVeManometreEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = oksijenTupuVeManometreEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] tansiyonAletiVeSteteskopTR = { "Dijital Tansiyon Aletleri", "Klasik Tansiyon Aletleri", "Steteskoplar", "T.Aleti Yedek Parçaları" };
            string[] tansiyonAletiVeSteteskopEN = { "Digital Sphygmomanometers", "Classic Sphygmomanometers", "Stethoscopes", "Sphygmomanometers Spare Parts" };
            string[] tansiyonAletiVeSteteskopRU = { "Цифровые сфигмоманометры", "Классические сфигмоманометры", "стетоскопы", "Запасные части сфигмоманометров" };

            int tansiyonAletiVeSteteskopId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Tansiyon Aletleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == tansiyonAletiVeSteteskopId).First().SubCategories.Any())
            {
                for (int i = 0; i < tansiyonAletiVeSteteskopTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = tansiyonAletiVeSteteskopTR[i],
                        NameEN = tansiyonAletiVeSteteskopEN[i],
                        NameRU = tansiyonAletiVeSteteskopRU[i],
                        MiddleCategoryId = tansiyonAletiVeSteteskopId,
                        SubCategoryNameUrlTR = tansiyonAletiVeSteteskopTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = tansiyonAletiVeSteteskopEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = tansiyonAletiVeSteteskopEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] tibbiCihazlarTR = { "Alın Aynaları", "Aspiratör Cihazları", "Beslenme Cihazları", "Çeşitli Cihazlar", "Defibrilatörler", "Diyapozonlar", "Doppler", "EKG Cihazları", "Hasta Başı Monitörleri", "Iyontoforez Cihazları", "İnfüzyon Pompası", "İşitme Cihazları", "Kompresyon Cihazları", "Konsantratörler", "Laringoskop Setleri", "Otoskoplar-Oftalmaskoplar", "Parafin Cihazı", "pH - Temp Metreler", "Pulse Oksimetreler", "Refleks Çekiciler", "Santrifüj Cihazları", "Şeker Ölçüm Cihazları", "Tansiyon Holterleri", "Tromel", "Ventilatörler" };

            string[] tibbiCihazlarEN = { "Forehead Mirrors", "Aspirator Devices", "Feeding Devices", "Various Devices", "Defibrillators", "Tuning forks", "Doppler", "ECG Devices", "Bedside Monitors", "Iontophoresis Devices", "Infusion Pump", "Hearing Devices", "Compression Devices", "Concentrators", "Laryngoscope Sets", "Otoscopes-Oftalmoskops", "Paraffin Appliance", "pH-Temp Meters", "Pulse Oximeters", "Reflex Tractor", "Centrifuge Devices", "Glucometers", "Blood Pressure Holters", "Trammel", "Ventilators" };

            string[] tibbiCihazlarRU = { "Лоб Зеркала", "Аспиратор", "Устройства для кормления", "Различные устройства", "Дефибрилляторы", "Камертоны", "допплер", "ЭКГ устройства", "Прикроватные мониторы", "Устройства для ионофореза", "Инфузионный насос", "Слуховые аппараты", "Компрессионные устройства", "Концентраторы", "Наборы ларингоскопов", "Офтальмоскопия", "Парафиновый прибор", "измерители pH-температуры", "Пульсоксиметры", "Рефлекс трактор", "Центрифуги", "Глюкометры", "Холтеры артериального давления", "штангенциркуль", "Вентиляторы" };

            int tibbiCihazlarId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Tıbbi Cihazlar")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == tibbiCihazlarId).First().SubCategories.Any())
            {
                for (int i = 0; i < tibbiCihazlarTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = tibbiCihazlarTR[i],
                        NameEN = tibbiCihazlarEN[i],
                        NameRU = tibbiCihazlarRU[i],
                        MiddleCategoryId = tibbiCihazlarId,
                        SubCategoryNameUrlTR = tibbiCihazlarTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = tibbiCihazlarEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = tibbiCihazlarEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }
            #endregion

            #region Hasta Bakıım Ürünleri

            string[] diyabetUrunleriTR = { "Diyabet Ayakkabısı", "İnsülin İğneleri", "Lansetler", "Şeker Ölçüm Cihazları", "Şeker Ölçüm Stripleri" };
            string[] diyabetUrunleriEN = { "Diabetes Shoes", "Insulin Needles", "Lancets", "Glucometers", "Glucometer Strips" };
            string[] diyabetUrunleriRU = { "Диабет Обувь", "Иглы инсулина", "Ланцеты", "Глюкометры", "Глюкометр Полоски" };

            int diyabetUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Diyabet Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == diyabetUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < diyabetUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = diyabetUrunleriTR[i],
                        NameEN = diyabetUrunleriEN[i],
                        NameRU = diyabetUrunleriRU[i],
                        MiddleCategoryId = diyabetUrunleriId,
                        SubCategoryNameUrlTR = diyabetUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = diyabetUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = diyabetUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                        HeadTitleTR = "Türkçe meta Title",
                        HeadTitleEN = "İngilizce meta Title",
                        HeadTitleRU = "Rusça meta Title"
                    });
                }
            }

            string[] gogusAlerjisiUrunleriTR = { "Anti-Mite Tekstil Ürünleri", "Chamberlar", "Çeşitli Ürünler" };
            string[] gogusAlerjisiUrunleriEN = { "Anti-Mite Textile Products", "Chambers", "Miscellaneous Products" };
            string[] gogusAlerjisiUrunleriRU = { "Анти-клещевые текстильные изделия", "Камеры", "Разные продукты" };

            int gogusAlerjisiUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Göğüs Alerjisi Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == gogusAlerjisiUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < gogusAlerjisiUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = gogusAlerjisiUrunleriTR[i],
                        NameEN = gogusAlerjisiUrunleriEN[i],
                        NameRU = gogusAlerjisiUrunleriRU[i],
                        MiddleCategoryId = gogusAlerjisiUrunleriId,
                        SubCategoryNameUrlTR = gogusAlerjisiUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = gogusAlerjisiUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = gogusAlerjisiUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, 
                        HeadDescriptionTR = "Türkçe meta Description", 
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description"
                    });
                }

            }

            string[] hastaBezleriTR = { "Bel Bantlı Bez", "Çocuk Bezleri", "Külot", "Mesane Pedi ve Ara Bezi", "Yatak Koruyucu" };
            string[] hastaBezleriEN = { "Waistband Diapers", "Diapers", "Panties", "Bladder Pad and Patient Gland", "Mattress Protector" };
            string[] hastaBezleriRU = { "Подгузник с поясом", "Подгузники", "трусы", "Подушка мочевого пузыря и железа пациента", "Ятак Коруюку" };

            int hastaBezleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Hasta Bezleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == hastaBezleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < hastaBezleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = hastaBezleriTR[i],
                        NameEN = hastaBezleriEN[i],
                        NameRU = hastaBezleriRU[i],
                        MiddleCategoryId = hastaBezleriId,
                        SubCategoryNameUrlTR = hastaBezleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = hastaBezleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = hastaBezleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, 
                        HeadDescriptionTR = "Türkçe meta Description", 
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }


            string[] solunumDestekUrunleriTR = { "Chamberlar", "Sarf Malzeme, Yedek Parça" };
            string[] solunumDestekUrunleriEN = { "Chambers", "Supplies, Spare Parts" };
            string[] solunumDestekUrunleriRU = { "Камеры", "предметы снабжения, Запасные части" };

            int solunumDestekUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Solunum Destek Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == solunumDestekUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < solunumDestekUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = solunumDestekUrunleriTR[i],
                        NameEN = solunumDestekUrunleriEN[i],
                        NameRU = solunumDestekUrunleriRU[i],
                        MiddleCategoryId = solunumDestekUrunleriId,
                        SubCategoryNameUrlTR = solunumDestekUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = solunumDestekUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = solunumDestekUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, 
                        HeadDescriptionTR = "Türkçe meta Description", 
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }

            string[] tekerlekliSandalyelerTR = { "Akülü Sandalyeler", "Manuel Sandalyeler" };
            string[] tekerlekliSandalyelerEN = { "Battery Powered Wheelchairs", "Manual Wheelchairs" };
            string[] tekerlekliSandalyelerRU = { "инвалидные коляски с батарейным питанием", "Ручные инвалидные коляски" };

            int tekerlekliSandalyelerId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Tekerlekli Sandalyeler")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == tekerlekliSandalyelerId).First().SubCategories.Any())
            {
                for (int i = 0; i < tekerlekliSandalyelerTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = tekerlekliSandalyelerTR[i],
                        NameEN = tekerlekliSandalyelerEN[i],
                        NameRU = tekerlekliSandalyelerRU[i],
                        MiddleCategoryId = tekerlekliSandalyelerId,
                        SubCategoryNameUrlTR = tekerlekliSandalyelerTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = tekerlekliSandalyelerEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = tekerlekliSandalyelerEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, 
                        HeadDescriptionTR = "Türkçe meta Description", 
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }



            string[] tibbiTekstilUrunleriTR = { "Alezler", "Anti-Mite Ürünler" };
            string[] tibbiTekstilUrunleriEN = { "Undersheets", "Anti-Mite Products" };
            string[] tibbiTekstilUrunleriRU = { "Подшерсток", "Анти-Клещ Продукты" };

            int tibbiTekstilUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Tıbbi Tekstil Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == tibbiTekstilUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < tibbiTekstilUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = tibbiTekstilUrunleriTR[i],
                        NameEN = tibbiTekstilUrunleriEN[i],
                        NameRU = tibbiTekstilUrunleriRU[i],
                        MiddleCategoryId = tibbiTekstilUrunleriId,
                        SubCategoryNameUrlTR = tibbiTekstilUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = tibbiTekstilUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = tibbiTekstilUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, 
                        HeadDescriptionTR = "Türkçe meta Description", 
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }


            string[] yaraBakimUrunleriTR = { "Ostomi Bakım Ürünleri", "Steril Yara Bantları", "Yara Bakım Ürünleri" };
            string[] yaraBakimUrunleriEN = { "Ostomy Care Products", "Sterile Wound Tapes", "Wound Care Products" };
            string[] yaraBakimUrunleriRU = { "Средства по уходу за стомой", "Стерильные ленты", "Средства для ухода за раной" };

            int yaraBakimUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Yara Bakım Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == yaraBakimUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < yaraBakimUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = yaraBakimUrunleriTR[i],
                        NameEN = yaraBakimUrunleriEN[i],
                        NameRU = yaraBakimUrunleriRU[i],
                        MiddleCategoryId = yaraBakimUrunleriId,
                        SubCategoryNameUrlTR = yaraBakimUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = yaraBakimUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = yaraBakimUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, 
                        HeadDescriptionTR = "Türkçe meta Description",
                        HeadDescriptionEN = "İngilizce meta Description",
                        HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }
            #endregion

            #region Kişisel Bakım ve Sağlık Ürünleri

            string[] ayakSagligiUrunleriTR = { "Ayak Bakım Ürünleri", "Ayak Tedavi Ürünleri", "Ayakkabılar ve Terlikler", "Tabanlıklar", "Topukluklar" };
            string[] ayakSagligiUrunleriEN = { "Foot Care Products", "Foot Treatment Products", "Shoes and Slippers", "Insole", "Heel inserts" };
            string[] ayakSagligiUrunleriRU = { "Средства по уходу за ногами", "Продукты для ухода за ногами", "Обувь и тапочки", "стелька", "пятки вкладыши" };

            int ayakSagligiUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Ayak Sağlığı Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == ayakSagligiUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < ayakSagligiUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = ayakSagligiUrunleriTR[i],
                        NameEN = ayakSagligiUrunleriEN[i],
                        NameRU = ayakSagligiUrunleriRU[i],
                        MiddleCategoryId = ayakSagligiUrunleriId,
                        SubCategoryNameUrlTR = ayakSagligiUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = ayakSagligiUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = ayakSagligiUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }


            string[] varisCoraplariTR = { "Kesikli Kompresör Cihazları", "Diz Altı", "Diz Üstü", "Hamile Külotlu", "Kasığa Kadar", "Külotlu", "Ülser-X Dizaltı" };
            string[] varisCoraplariEN = { "Discrete Compressor Devices", "Under the knees", "Above Knee", "Pregnant Pantyhose", "Up To The Groin", "Panty", "Ulcer-X Knee" };
            string[] varisCoraplariRU = { "Дискретные компрессорные устройства", "Ниже колена", "Выше колена", "Беременные Колготки", "До паха", "колготки", "Ulcer-X колено" };


            int varisCoraplariId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Varis Çorapları")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == varisCoraplariId).First().SubCategories.Any())
            {
                for (int i = 0; i < varisCoraplariTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = varisCoraplariTR[i],
                        NameEN = varisCoraplariEN[i],
                        NameRU = varisCoraplariRU[i],
                        MiddleCategoryId = varisCoraplariId,
                        SubCategoryNameUrlTR = varisCoraplariTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = varisCoraplariEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = varisCoraplariEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }
            #endregion


            #region Ortopedi Ürünleri
            string[] fizikTedaviUrunleriTR = { "Çeşitli Ürünler", "El Terapisi", "Fitness ve Pilates Ürünleri", "Isıtıcı Tekstil Ürünler", "Lenf ve Ödem Bandajları", "Ortopedik Yastıklar & Simitler", "Soğuk-Sıcak Jel/Termofor", "Yardımcı Ürünler", "Zayıflama Kemerleri" };
            string[] fizikTedaviUrunleriEN = { "Miscellaneous Products", "Hand Therapy", "Fitness and Pilates Products", "Heating Textile Products", "Lymph and Edema Bandages", "Orthopedic Pillows and Bagels", "Cold-Hot Gel/Thermophore", "Auxiliary Products", "Slimming Belts" };
            string[] fizikTedaviUrunleriRU = { "Разные продукты", "Ручная терапия", "Фитнес и пилатес", "Отопление Текстильные изделия", "Лимфатические и отечные повязки", "Ортопедические подушки и бублики", "Холодно-Горячий Гель/Термофор", "Вспомогательные продукты", "Пояса для похудения" };

            int fizikTedaviUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Fizik Tedavi Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == fizikTedaviUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < fizikTedaviUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = fizikTedaviUrunleriTR[i],
                        NameEN = fizikTedaviUrunleriEN[i],
                        NameRU = fizikTedaviUrunleriRU[i],
                        MiddleCategoryId = fizikTedaviUrunleriId,
                        SubCategoryNameUrlTR = fizikTedaviUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = fizikTedaviUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = fizikTedaviUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }

            string[] sporcuSagligiUrunleriTR = { "Ayak/Bilek Ağırlıkları", "Çeşitli Ürünler", "Nabız Ölçer" };
            string[] sporcuSagligiUrunleriEN = { "Foot/Ankle Weights", "Miscellaneous Products", "Heart Rate Meter" };
            string[] sporcuSagligiUrunleriRU = { "Вес ноги/лодыжки", "Разные продукты", "Измеритель сердечного ритма" };

            int sporcuSagligiUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Sporcu Sağlığı Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == sporcuSagligiUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < sporcuSagligiUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = sporcuSagligiUrunleriTR[i],
                        NameEN = sporcuSagligiUrunleriEN[i],
                        NameRU = sporcuSagligiUrunleriRU[i],
                        MiddleCategoryId = sporcuSagligiUrunleriId,
                        SubCategoryNameUrlTR = sporcuSagligiUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = sporcuSagligiUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = sporcuSagligiUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }
            #endregion

            #region Sarf Malzemeleri

            string[] aileHekimligiMalzemeleriTR = { "Aşı Nakil Çantaları", "Tıbbi Sarf Malzemeleri" };
            string[] aileHekimligiMalzemeleriEN = { "Vaccine Transport Bags", "Medical Supplies" };
            string[] aileHekimligiMalzemeleriRU = { "Сумки для перевозки вакцин", "Медицинские товары" };

            int aileHekimligiMalzemeleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("Aile Hekimliği Malzemeleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == aileHekimligiMalzemeleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < aileHekimligiMalzemeleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = aileHekimligiMalzemeleriTR[i],
                        NameEN = aileHekimligiMalzemeleriEN[i],
                        NameRU = aileHekimligiMalzemeleriRU[i],
                        MiddleCategoryId = aileHekimligiMalzemeleriId,
                        SubCategoryNameUrlTR = aileHekimligiMalzemeleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = aileHekimligiMalzemeleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = aileHekimligiMalzemeleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }

            string[] ilkYardimUrunleriTR = { "Ateller", "İlkyardım Çantaları ve Ecza Dolapları", "İlk Yardım Malzemeleri", "Sedyeler", "Soğuk/Sıcak Kompres", "Turnikeler" };
            string[] ilkYardimUrunleriEN = { "Splints", "First Aid Bags and Medicine Cabinets", "First Aid Supplies", "Stretchers", "Cold/Hot Compress", "Turnstiles" };
            string[] ilkYardimUrunleriRU = { "соломка", "Сумки первой помощи и аптечки", "Предметы первой помощи", "носилки", "Холодный/Горячий Компресс", "турникеты" };

            int ilkYardimUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("İlk Yardım Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == ilkYardimUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < ilkYardimUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = ilkYardimUrunleriTR[i],
                        NameEN = ilkYardimUrunleriEN[i],
                        NameRU = ilkYardimUrunleriRU[i],
                        MiddleCategoryId = ilkYardimUrunleriId,
                        SubCategoryNameUrlTR = ilkYardimUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = ilkYardimUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = ilkYardimUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }


            string[] kbbUrunleriTR = { "Kulak Sarf Malzemeleri", "Burun Sarf Malzemeleri", "Boğaz Sarf Malzemeleri", "Diapozon Setler" };
            string[] kbbUrunleriEN = { "Ear Consumables", "Nose Consumables", "Throat Consumables", "Diapozon Sets" };
            string[] kbbUrunleriRU = { "Расходные материалы для ушей", "Расходные материалы для носа", "Расходные материалы для горла", "Наборы Диапозонов" };

            int kbbUrunleriId = _midCateRepo.FindEntities(x => x.NameTR.Equals("KBB Ürünleri")).First().Id;

            if (!_midCateRepo.FindEntities(x => x.Id == kbbUrunleriId).First().SubCategories.Any())
            {
                for (int i = 0; i < kbbUrunleriTR.Length; i++)
                {
                    _subCateRepo.CreateEntity(new SubCategory
                    {
                        NameTR = kbbUrunleriTR[i],
                        NameEN = kbbUrunleriEN[i],
                        NameRU = kbbUrunleriRU[i],
                        MiddleCategoryId = kbbUrunleriId,
                        SubCategoryNameUrlTR = kbbUrunleriTR[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlEN = kbbUrunleriEN[i].ConvertToFriendlyUrl(),
                        SubCategoryNameUrlRU = kbbUrunleriEN[i].ConvertToFriendlyUrl(),
                        CategoryType = CategoryType.sub, HeadDescriptionTR = "Türkçe meta Description", HeadDescriptionEN = "İngilizce meta Description",HeadDescriptionRU = "Rusça meta Description",
                    });
                }
            }
            #endregion
        }

        public void PopulateSliders()
        {
            Random rnd = new Random();
            if (!_sliderRepo.GetAllEntities().Any())
            {
                // türkçe slider lar
                for (int i = 1; i < 8; i++)
                {
                    var productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                    var pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();
                    do
                    {
                        productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                        pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();

                    } while (pro2.Count == 0);

                    var brands = _brandRepo.GetAllEntities().ToList();
                    brands.ShuffleMyList();

                    var type = FakeData.EnumData.GetElement<SliderTargetType>();
                    int typee = (int)type;

                    Slider slider = new Slider();
                    if (typee==0)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                        slider.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                        slider.TargetSubCategoryId = pro2[0].SubCategoryId;
                    }
                    if (typee==1)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                    }
                    if (typee==2)
                    {
                        slider.TargetBrandId = brands[0].Id;
                    }
                    if (typee==3)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                        slider.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                    }
                    if (typee==4)
                    {
                        slider.TargetProductId = pro2[0].Id;
                    }
                    slider.PhotoFileName = $"{i}.jpg";
                    slider.ThumbFileName = $"{i}_thumb.jpg";
                    slider.PhotoAltTag = "SliderAltTagiTürkçe-" + i.ToString();
                    slider.Culture = "tr";
                    slider.SliderTargetType = type;
                    slider.SliderHref = 
                          typee == 2 ? $"/tr/markalar/{brands[0].BandNameUrl}/sayfa/1" 
                        : typee == 1 ? $"/tr/kategoriler/{pro2[0].TopCategory.TopCategoryNameUrlTR}/sayfa/1" 
                        : typee == 3 ? $"/tr/kategoriler/{pro2[0].TopCategory.TopCategoryNameUrlTR}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlTR}/sayfa/1" 
                        : typee == 0 ? $"/tr/kategoriler/{pro2[0].TopCategory.TopCategoryNameUrlTR}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlTR}/{pro2[0].SubCategory.SubCategoryNameUrlTR}/sayfa/1" 
                        : $"/tr/urun-detay/{brands[0].BandNameUrl}/{pro2[0].TopCategory.TopCategoryNameUrlTR}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlTR}/{pro2[0].SubCategory.SubCategoryNameUrlTR}/{pro2[0].ProductNameUrlTR}";
                    _sliderRepo.CreateEntity(slider);
                }

                // ingilizce slider lar
                for (int i = 1; i < 8; i++)
                {
                    var productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                    var pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();
                    do
                    {
                        productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                        pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();

                    } while (pro2.Count == 0);

                    var brands = _brandRepo.GetAllEntities().ToList();
                    brands.ShuffleMyList();

                    //string sliderMainPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "SliderImages", "tr");
                    var type = FakeData.EnumData.GetElement<SliderTargetType>();
                    int typee = (int)type;
                    brands.ShuffleMyList();

                    Slider slider = new Slider();
                    if (typee == 0)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                        slider.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                        slider.TargetSubCategoryId = pro2[0].SubCategoryId;
                    }
                    if (typee == 1)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                    }
                    if (typee == 2)
                    {
                        slider.TargetBrandId = brands[0].Id;
                    }
                    if (typee == 3)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                        slider.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                    }
                    if (typee == 4)
                    {
                        slider.TargetProductId = pro2[0].Id;
                    }
                    slider.PhotoFileName = $"{i}.jpg";
                    slider.ThumbFileName = $"{i}_thumb.jpg";
                    slider.PhotoAltTag = "SliderAltTagiİngilizce-" + i.ToString();
                    slider.Culture = "en";
                    slider.SliderTargetType = type;
                    slider.SliderHref = 
                        typee == 2 ? $"/en/brands/{brands[0].BandNameUrl}/page/1" 
                        : typee == 1 ? $"/en/categories/{pro2[0].TopCategory.TopCategoryNameUrlEN}/page/1" 
                        : typee == 3 ? $"/en/categories/{pro2[0].TopCategory.TopCategoryNameUrlEN}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlEN}/page/1" 
                        : typee == 0 ? $"/en/categories/{pro2[0].TopCategory.TopCategoryNameUrlEN}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlEN}/{pro2[0].SubCategory.SubCategoryNameUrlEN}/page/1" 
                        : $"/en/product-detail/{brands[0].BandNameUrl}/{pro2[0].TopCategory.TopCategoryNameUrlEN}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlEN}/{pro2[0].SubCategory.SubCategoryNameUrlEN}/{pro2[0].ProductNameUrlEN}";
                    _sliderRepo.CreateEntity(slider);
                }

                // rusça slider lar
                for (int i = 1; i < 8; i++)
                {
                    var productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                    var pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();
                    do
                    {
                        productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                        pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();

                    } while (pro2.Count == 0);

                    var brands = _brandRepo.GetAllEntities().ToList();
                    brands.ShuffleMyList();

                    //string sliderMainPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "SliderImages", "tr");
                    var type = FakeData.EnumData.GetElement<SliderTargetType>();
                    int typee = (int)type;
                    brands.ShuffleMyList();

                    Slider slider = new Slider();
                    if (typee == 0)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                        slider.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                        slider.TargetSubCategoryId = pro2[0].SubCategoryId;
                    }
                    if (typee == 1)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                    }
                    if (typee == 2)
                    {
                        slider.TargetBrandId = brands[0].Id;
                    }
                    if (typee == 3)
                    {
                        slider.TargetTopCategoryId = pro2[0].TopCategoryId;
                        slider.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                    }
                    if (typee == 4)
                    {
                        slider.TargetProductId = pro2[0].Id;
                    }
                    slider.PhotoFileName = $"{i}.jpg";
                    slider.ThumbFileName = $"{i}_thumb.jpg";
                    slider.PhotoAltTag = "SliderAltTagiRusça-" + i.ToString();
                    slider.Culture = "ru";
                    slider.SliderTargetType = type;
                    slider.SliderHref = 
                        typee == 2 ? $"/ru/brands/{brands[0].BandNameUrl}/page/1" 
                        : typee == 1 ? $"/ru/categories/{pro2[0].TopCategory.TopCategoryNameUrlRU}/page/1" 
                        : typee == 3 ? $"/ru/categories/{pro2[0].TopCategory.TopCategoryNameUrlRU}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlRU}/page/1" 
                        : typee == 0 ? $"/ru/categories/{pro2[0].TopCategory.TopCategoryNameUrlRU}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlRU}/{pro2[0].SubCategory.SubCategoryNameUrlRU}/page/1" 
                        : $"/ru/product-detail/{brands[0].BandNameUrl}/{pro2[0].TopCategory.TopCategoryNameUrlRU}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlRU}/{pro2[0].SubCategory.SubCategoryNameUrlRU}/{pro2[0].ProductNameUrlRU}";
                    _sliderRepo.CreateEntity(slider);
                }

            }
        }

        public void PopulateMiniSliders()
        {
            if (!_miniSliderRepo.GetAllEntities().Any())
            {
                var products = _productRepo.GetAllEntities().ToList();

                // türkçe mini slider lar
                for (int i = 1; i < 5; i++)
                {
                    products.ShuffleMyList();

                    var pro = _productRepo.GetProductWithNameUrl(products[0].ProductNameUrlTR, "tr");

                    _miniSliderRepo.CreateEntity(new MiniSlider
                    {
                        PhotoFileName = $"{i}.jpg",
                        PhotoAltTag = "miniSliderAltTagiTürkçe-" + i.ToString(),
                        Culture = "tr",
                        TargetProductId= pro.Id,
                        TargetProductName = pro.NameTR,
                        SliderHref = pro.SubCategoryId != null ? $"/tr/urun-detay/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlTR}/{pro.MiddleCategory.MiddleCategoryNameUrlTR}/{pro.SubCategory.SubCategoryNameUrlTR}/{pro.ProductNameUrlTR}" : pro.MiddleCategoryId != null ? $"/tr/urun-detay/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlTR}/{pro.MiddleCategory.MiddleCategoryNameUrlTR}/{pro.ProductNameUrlTR}" : $"/tr/urun-detay/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlTR}/{pro.ProductNameUrlTR}"
                    });
                }
                // ingilizce mini slider lar
                for (int i = 1; i < 5; i++)
                {
                    products.ShuffleMyList();

                    var pro = _productRepo.GetProductWithNameUrl(products[0].ProductNameUrlEN, "en");

                    _miniSliderRepo.CreateEntity(new MiniSlider
                    {
                        PhotoFileName = $"{i}.jpg",
                        PhotoAltTag = "miniSliderAltTagiİngilizce-" + i.ToString(),
                        Culture = "en",
                        TargetProductId = pro.Id,
                        TargetProductName = pro.NameTR,
                        SliderHref = pro.SubCategoryId != null ? $"/en/product-detail/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlEN}/{pro.MiddleCategory.MiddleCategoryNameUrlEN}/{pro.SubCategory.SubCategoryNameUrlEN}/{pro.ProductNameUrlEN}" : pro.MiddleCategoryId != null ? $"/en/product-detail/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlEN}/{pro.MiddleCategory.MiddleCategoryNameUrlEN}/{pro.ProductNameUrlEN}" : $"/en/product-detail/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlEN}/{pro.ProductNameUrlEN}"
                    });
                }

                // rusça mini slider lar
                for (int i = 1; i < 5; i++)
                {
                    products.ShuffleMyList();

                    var pro = _productRepo.GetProductWithNameUrl(products[0].ProductNameUrlRU, "ru");

                    _miniSliderRepo.CreateEntity(new MiniSlider
                    {
                        PhotoFileName = $"{i}.jpg",
                        PhotoAltTag = "miniSliderAltTagiRusça-" + i.ToString(),
                        Culture = "ru",
                        TargetProductId = pro.Id,
                        TargetProductName= pro.NameTR,
                        SliderHref = pro.SubCategoryId != null ? $"/ru/product-detail/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlRU}/{pro.MiddleCategory.MiddleCategoryNameUrlRU}/{pro.SubCategory.SubCategoryNameUrlRU}/{pro.ProductNameUrlRU}" : pro.MiddleCategoryId != null ? $"/ru/product-detail/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlRU}/{pro.MiddleCategory.MiddleCategoryNameUrlRU}/{pro.ProductNameUrlRU}" : $"/ru/product-detail/{pro.Brand.BandNameUrl}/{pro.TopCategory.TopCategoryNameUrlRU}/{pro.ProductNameUrlRU}"
                    });
                }
            }
        }

        public void PopulateAdProducts()
        {
            if (!_adproRepo.GetAllEntities().Any())
            {
                Random rnd = new Random();
                // türkçe adproductlar
                for (int i = 1; i < 7; i++)
                {
                    var productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                    var pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();
                    do
                    {
                        productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                        pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();

                    } while (pro2.Count == 0);

                    var brands = _brandRepo.GetAllEntities().ToList();
                    brands.ShuffleMyList();

                    var type = FakeData.EnumData.GetElement<SliderTargetType>();
                    int typee = (int)type;
                    brands.ShuffleMyList();

                    AdProduct pro = new AdProduct();
                    if (typee == 0)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                        pro.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                        pro.TargetSubCategoryId = pro2[0].SubCategoryId;
                    }
                    if (typee == 1)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                    }
                    if (typee == 2)
                    {
                        pro.TargetBrandId = brands[0].Id;
                    }
                    if (typee == 3)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                        pro.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                    }
                    if (typee == 4)
                    {
                        pro.TargetProductId = pro2[0].Id;
                    }
                    pro.PhotoFileName = $"{i}.jpg";
                    pro.PhotoAltTag = "AdProductAltTagiTürkçe-" + i.ToString();
                    pro.Culture = "tr";
                    pro.AdproTargetType= type;
                    pro.AdproHref = typee == 0 ? $"/tr/markalar/{brands[0].BandNameUrl}/sayfa/1" : typee == 1 ? $"/tr/kategoriler/{pro2[0].TopCategory.TopCategoryNameUrlTR}/sayfa/1" : typee == 2 ? $"/tr/kategoriler/{pro2[0].TopCategory.TopCategoryNameUrlTR}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlTR}/sayfa/1" : typee == 3 ? $"/tr/kategoriler/{pro2[0].TopCategory.TopCategoryNameUrlTR}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlTR}/{pro2[0].SubCategory.SubCategoryNameUrlTR}/sayfa/1" : $"/tr/urun-detay/{brands[0].BandNameUrl}/{pro2[0].TopCategory.TopCategoryNameUrlTR}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlTR}/{pro2[0].SubCategory.SubCategoryNameUrlTR}/{pro2[0].ProductNameUrlTR}";
                    _adproRepo.CreateEntity(pro);
                }

                // ingilizce adproductlar
                for (int i = 1; i < 7; i++)
                {
                    var productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                    var pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();
                    do
                    {
                        productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                        pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();

                    } while (pro2.Count == 0);

                    var brands = _brandRepo.GetAllEntities().ToList();
                    brands.ShuffleMyList();

                    //string sliderMainPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "SliderImages", "tr");
                    var type = FakeData.EnumData.GetElement<SliderTargetType>();
                    int typee = (int)type;
                    brands.ShuffleMyList();

                    AdProduct pro = new AdProduct();
                    if (typee == 0)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                        pro.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                        pro.TargetSubCategoryId = pro2[0].SubCategoryId;
                    }
                    if (typee == 1)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                    }
                    if (typee == 2)
                    {
                        pro.TargetBrandId = brands[0].Id;
                    }
                    if (typee == 3)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                        pro.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                    }
                    if (typee == 4)
                    {
                        pro.TargetProductId = pro2[0].Id;
                    }
                    pro.PhotoFileName = $"{i}.jpg";
                    pro.PhotoAltTag = "AdproductAltTagiİngilizce-" + i.ToString();
                    pro.Culture = "en";
                    pro.AdproTargetType = type;
                    pro.AdproHref = typee == 0 ? $"/en/brands/{brands[0].BandNameUrl}/page/1" : typee == 1 ? $"/en/categories/{pro2[0].TopCategory.TopCategoryNameUrlEN}/page/1" : typee == 2 ? $"/en/categories/{pro2[0].TopCategory.TopCategoryNameUrlEN}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlEN}/page/1" : typee == 3 ? $"/en/categories/{pro2[0].TopCategory.TopCategoryNameUrlEN}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlEN}/{pro2[0].SubCategory.SubCategoryNameUrlEN}/page/1" : $"/en/product-detail/{brands[0].BandNameUrl}/{pro2[0].TopCategory.TopCategoryNameUrlEN}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlEN}/{pro2[0].SubCategory.SubCategoryNameUrlEN}/{pro2[0].ProductNameUrlEN}";
                    _adproRepo.CreateEntity(pro);
                }

                // rusça adproductlar
                for (int i = 1; i < 7; i++)
                {
                    var productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                    var pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();
                    do
                    {
                        productList = _productRepo.GetProductsWithTopcateId(rnd.Next(1, _topCateRepo.GetAllEntities().Count() - 1));
                        pro2 = productList.Where(x => x.MiddleCategoryId != null && x.SubCategoryId != null).ToList();

                    } while (pro2.Count == 0);

                    var brands = _brandRepo.GetAllEntities().ToList();
                    brands.ShuffleMyList();

                    //string sliderMainPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "SliderImages", "tr");
                    var type = FakeData.EnumData.GetElement<SliderTargetType>();
                    int typee = (int)type;
                    brands.ShuffleMyList();

                    AdProduct pro = new AdProduct();
                    if (typee == 0)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                        pro.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                        pro.TargetSubCategoryId = pro2[0].SubCategoryId;
                    }
                    if (typee == 1)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                    }
                    if (typee == 2)
                    {
                        pro.TargetBrandId = brands[0].Id;
                    }
                    if (typee == 3)
                    {
                        pro.TargetTopCategoryId = pro2[0].TopCategoryId;
                        pro.TargetMiddleCategoryId = pro2[0].MiddleCategoryId;
                    }
                    if (typee == 4)
                    {
                        pro.TargetProductId = pro2[0].Id;
                    }
                    pro.PhotoFileName = $"{i}.jpg";
                    pro.PhotoAltTag = "AdproductAltTagiRusça-" + i.ToString();
                    pro.Culture = "ru";
                    pro.AdproTargetType = type;
                    pro.AdproHref = typee == 0 ? $"/ru/brands/{brands[0].BandNameUrl}/page/1" : typee == 1 ? $"/ru/categories/{pro2[0].TopCategory.TopCategoryNameUrlRU}/page/1" : typee == 2 ? $"/ru/categories/{pro2[0].TopCategory.TopCategoryNameUrlRU}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlRU}/page/1" : typee == 3 ? $"/ru/categories/{pro2[0].TopCategory.TopCategoryNameUrlRU}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlRU}/{pro2[0].SubCategory.SubCategoryNameUrlRU}/page/1" : $"/ru/product-detail/{brands[0].BandNameUrl}/{pro2[0].TopCategory.TopCategoryNameUrlRU}/{pro2[0].MiddleCategory.MiddleCategoryNameUrlRU}/{pro2[0].SubCategory.SubCategoryNameUrlRU}/{pro2[0].ProductNameUrlRU}";
                    _adproRepo.CreateEntity(pro);
                }

            }
        }

        public void PopulateBrands()
        {
            if (!_brandRepo.AnyEntity())
            {
                string[] markalar = { "3M", "Abena", "Bayer", "Beurer", "Bıçakçılar", "Canped", "Caretex", "Coloplast", "Depend", "Erler-Zimmer", "Firstmed", "Froximun", "Herdegen", "Kifidis", "Kimpa", "LAB-VET", "Medela", "Omron", "Philips", "Plasmed", "Plusmed", "Promedic", "Roche", "Selpak", "Smith&Nephew", "Turmed", "Tyco", "Vivocare", "Wollex" };

                Array.Sort(markalar);

                for (int i = 0; i < markalar.Length; i++)
                {
                    _brandRepo.CreateEntity(new Brand
                    {
                        BrandName = markalar[i],
                        PhotoFileName = $"{markalar[i]}.jpg",
                        PhotoAltTagTR = $"{markalar[i]} Ürünleri",
                        PhotoAltTagEN = $"{markalar[i]} Products",
                        PhotoAltTagRU = $"{markalar[i]} Товары",
                        MasterPageMetaTitleTR= $"{markalar[i]} meta title Türkçe",
                        MasterPageMetaTitleEN= $"{markalar[i]} meta title İngilizce",
                        MasterPageMetaTitleRU= $"{markalar[i]} meta title Rusça",
                        MasterPageMetaDescriptionTR= $"{markalar[i]} meta description Türkçe",
                        MasterPageMetaDescriptionEN= $"{markalar[i]} meta description İngilizce",
                        MasterPageMetaDescriptionRU= $"{markalar[i]} meta description Rusça",
                        BandNameUrl = markalar[i].ConvertToFriendlyUrl()
                    });
                }
            }
        }

        public void PopulateProducts()
        {
            if (!_productRepo.AnyEntity())
            {
                Random rnd = new Random();

                for (int i = 1; i <= 500; i++)
                {
                    int counter = rnd.Next(1, 11);
                    int counter2 = rnd.Next(1, 11);
                    int counter3 = rnd.Next(1, 11);
                    List<int> discountList = new List<int> { 5, 10, 15, 20, 30, 40, 50 };
                    bool hasMiddleCate = false;
                    bool hasSubCate = false;
                    int topCateId = rnd.Next(1, 11);
                    int midCateId = 0;
                    int subCateId = 0;

                    if (topCateId != 8 && topCateId != 10)
                    {
                        hasMiddleCate = true;
                    }
                    if (hasMiddleCate)
                    {
                        midCateId = MiddleCategoryFinder(topCateId);
                        var midCate = _midCateRepo.GetEntityById(midCateId);
                        if (midCate.HasSubCategories)
                        {
                            subCateId = SubCategoryFinder(midCateId);
                            hasSubCate = true;
                        }
                    }

                    if (midCateId == 0 && subCateId == 0)
                    {
                        discountList.ShuffleMyList();

                        Product product = new Product
                        {
                            BrandId = rnd.Next(1, 29),
                            NameTR = $"Ürün-{i} Adı",
                            NameEN = $"Product-{i} Name",
                            NameRU = $"название-{i} продукта",
                            ProductDescriptionTR = $"Ürün-{i} Açıklaması: Tekerleklı ve çelik gövdeli olarak tasaralanan ürünümüz, kontrol kumandası sayesinde baş, sırtlık ve ayak kısmı dahil ayarlanabilir. Imitasyon kayın, MDF veya Ahşap panolarla yatağınıza ayrı bır hava katabilirsiniz. Ek olarak Evrensel kromm ahşap lateral veya Lateral katlanır korkuluklarla tam teşekküllü bir yatak haline getirebilirsiniz, 3 motorlu halinde mevcuttur. Avrupa standartlarına uygun ve CE markajlı karyolalar,Herdegen tarafında 15 yıldır evde hasta bakımı için üretilmektedir. Tüm yataklarımız Fransada imal ediliyor ve yüksek kaliteli Linak motorlar ile donatılmıştır. Karyolalarımız kolayca katlanır ve hasta evine teslim edilir.Makas mekanizması ile tasarlanmış ve 3 motorla donatılmıştır. Yatan hastanın kalkış ve yatışları düşünülerek karyolanın baş kısmında hasta tutamağı(deveboynu) tutunarak kalkışlarda ve yatışlarda hastanın kuvvet almasını sağlar.",
                            ProductDescriptionEN = $"Product-{i} Description: Our product, which is designed with wheels and steel body, can be adjusted including the head, backrest and foot part thanks to its control control. You can add a separate air to your bed with imitation beech, MDF or Wooden boards. In addition, you can turn Universal chrome wooden lateral or Lateral folding rails into a full-fledged bed, available in 3 motors. CE-marked cots complying with European standards have been produced by Herdegen for 15 years at home for patient care. All our bearings are manufactured in France and equipped with high quality Linak engines. Our beds are easily folded and delivered to the patient home. They are designed with a scissors mechanism and equipped with 3 motors. By considering the departure and hospitalization of the inpatient, the patient's handle (gooseneck) is held at the head of the bed, allowing the patient to gain strength during departures and hospitalizations.",
                            ProductDescriptionRU = $"продукт-{i} описание: Наш продукт, который разработан с колесами и стальным корпусом, может быть отрегулирован, включая голову, спинку и ножку, благодаря своему управлению. Вы можете добавить отдельный воздух к вашей кровати с имитацией бука, МДФ или деревянных досок. Кроме того, вы можете превратить универсальные хромированные деревянные боковые или боковые откидные рельсы в полноценную кровать, доступную в 3 моторах. Кровати с маркировкой CE, соответствующие европейским стандартам, производятся Herdegen уже 15 лет на дому для ухода за пациентами. Все наши подшипники производятся во Франции и оснащены высококачественными двигателями Linak. Наши кровати легко складываются и доставляются к пациенту домой, они оснащены ножничным механизмом и оснащены 3 моторами. Принимая во внимание отправление и госпитализацию стационарного больного, ручка пациента (гусиная шея) удерживается в верхней части кровати, что позволяет пациенту набирать силу во время отъезда и госпитализации.",
                            HeadTitleTR= "Türkçe meta title tag",
                            HeadTitleEN= "İngilizce meta title tag",
                            HeadTitleRU= "Rusça meta title tag",
                            HeadDescriptionTR = "Türkçe meta description tag",
                            HeadDescriptionEN = "İngilizce meta description tag",
                            HeadDescriptionRU = "Rusça meta description tag",
                            ProductOfferType = counter<=3 ? FakeData.EnumData.GetElement<ProductOfferType>() : ProductOfferType.Nothing,
                            TopCategoryId = topCateId,
                            HasNewBadge = counter2 <= 3 ? true : false,
                            IsFreeShipping = counter3 <= 4 ? true : false,
                            DiscountRate = discountList[0],
                            ProductCode = rnd.Next(1000, 5000).ToString(),
                            StockNumber = rnd.Next(5000, 9999),
                            PhotoAltTagEN= "İngilizce Ürün Fotoğraf Alt Tagi",
                            PhotoAltTagTR= "Türkçe Ürün Fotoğraf Alt Tagi",
                            PhotoAltTagRU= "Rusça Ürün Fotoğraf Alt Tagi",
                            
                        };
                        product.ProductNameUrlTR = product.NameTR.ConvertToFriendlyUrl();
                        product.ProductNameUrlEN = product.NameEN.ConvertToFriendlyUrl();
                        product.ProductNameUrlRU = product.NameEN.ConvertToFriendlyUrl();

                        _productRepo.CreateEntity(product);
                    }

                    else if (midCateId != 0 && subCateId != 0)
                    {
                        discountList.ShuffleMyList();

                        Product product = new Product
                        {
                            BrandId = rnd.Next(1, 21),
                            NameTR = $"Ürün-{i} Adı",
                            NameEN = $"Product-{i} Name",
                            NameRU = $"название-{i} продукта",
                            ProductDescriptionTR = $"Ürün-{i} Açıklaması: Tekerleklı ve çelik gövdeli olarak tasaralanan ürünümüz, kontrol kumandası sayesinde baş, sırtlık ve ayak kısmı dahil ayarlanabilir. Imitasyon kayın, MDF veya Ahşap panolarla yatağınıza ayrı bır hava katabilirsiniz. Ek olarak Evrensel kromm ahşap lateral veya Lateral katlanır korkuluklarla tam teşekküllü bir yatak haline getirebilirsiniz, 3 motorlu halinde mevcuttur. Avrupa standartlarına uygun ve CE markajlı karyolalar,Herdegen tarafında 15 yıldır evde hasta bakımı için üretilmektedir. Tüm yataklarımız Fransada imal ediliyor ve yüksek kaliteli Linak motorlar ile donatılmıştır. Karyolalarımız kolayca katlanır ve hasta evine teslim edilir.Makas mekanizması ile tasarlanmış ve 3 motorla donatılmıştır. Yatan hastanın kalkış ve yatışları düşünülerek karyolanın baş kısmında hasta tutamağı(deveboynu) tutunarak kalkışlarda ve yatışlarda hastanın kuvvet almasını sağlar.",
                            ProductDescriptionEN = $"Product-{i} Description: Our product, which is designed with wheels and steel body, can be adjusted including the head, backrest and foot part thanks to its control control. You can add a separate air to your bed with imitation beech, MDF or Wooden boards. In addition, you can turn Universal chrome wooden lateral or Lateral folding rails into a full-fledged bed, available in 3 motors. CE-marked cots complying with European standards have been produced by Herdegen for 15 years at home for patient care. All our bearings are manufactured in France and equipped with high quality Linak engines. Our beds are easily folded and delivered to the patient home. They are designed with a scissors mechanism and equipped with 3 motors. By considering the departure and hospitalization of the inpatient, the patient's handle (gooseneck) is held at the head of the bed, allowing the patient to gain strength during departures and hospitalizations.",
                            ProductDescriptionRU = $"продукт-{i} описание: Наш продукт, который разработан с колесами и стальным корпусом, может быть отрегулирован, включая голову, спинку и ножку, благодаря своему управлению. Вы можете добавить отдельный воздух к вашей кровати с имитацией бука, МДФ или деревянных досок. Кроме того, вы можете превратить универсальные хромированные деревянные боковые или боковые откидные рельсы в полноценную кровать, доступную в 3 моторах. Кровати с маркировкой CE, соответствующие европейским стандартам, производятся Herdegen уже 15 лет на дому для ухода за пациентами. Все наши подшипники производятся во Франции и оснащены высококачественными двигателями Linak. Наши кровати легко складываются и доставляются к пациенту домой, они оснащены ножничным механизмом и оснащены 3 моторами. Принимая во внимание отправление и госпитализацию стационарного больного, ручка пациента (гусиная шея) удерживается в верхней части кровати, что позволяет пациенту набирать силу во время отъезда и госпитализации.",
                            HeadTitleTR = "Türkçe meta title tag",
                            HeadTitleEN = "İngilizce meta title tag",
                            HeadTitleRU = "Rusça meta title tag",
                            HeadDescriptionTR = "Türkçe meta description tag",
                            HeadDescriptionEN = "İngilizce meta description tag",
                            HeadDescriptionRU = "Rusça meta description tag",
                            ProductOfferType = FakeData.EnumData.GetElement<ProductOfferType>(),
                            TopCategoryId = topCateId,
                            MiddleCategoryId = midCateId,
                            SubCategoryId = subCateId,
                            HasNewBadge = counter2 <= 3 ? true : false,
                            IsFreeShipping = counter3 <= 4 ? true : false,
                            DiscountRate = discountList[0],
                            ProductCode = rnd.Next(1000, 5000).ToString(),
                            StockNumber = rnd.Next(5000, 9999),
                            PhotoAltTagEN = "İngilizce Ürün Fotoğraf Alt Tagi",
                            PhotoAltTagTR = "Türkçe Ürün Fotoğraf Alt Tagi",
                            PhotoAltTagRU = "Rusça Ürün Fotoğraf Alt Tagi",
                        };
                        product.ProductNameUrlTR = product.NameTR.ConvertToFriendlyUrl();
                        product.ProductNameUrlEN = product.NameEN.ConvertToFriendlyUrl();
                        product.ProductNameUrlRU = product.NameEN.ConvertToFriendlyUrl();
                        
                        _productRepo.CreateEntity(product);
                    }

                    else if (midCateId != 0 && subCateId == 0)
                    {
                        discountList.ShuffleMyList();

                        Product product = new Product
                        {
                            BrandId = rnd.Next(1, 21),
                            NameTR = $"Ürün-{i} Adı",
                            NameEN = $"Product-{i} Name",
                            NameRU = $"название-{i} продукта",
                            ProductDescriptionTR = $"Ürün-{i} Açıklaması: Tekerleklı ve çelik gövdeli olarak tasaralanan ürünümüz, kontrol kumandası sayesinde baş, sırtlık ve ayak kısmı dahil ayarlanabilir. Imitasyon kayın, MDF veya Ahşap panolarla yatağınıza ayrı bır hava katabilirsiniz. Ek olarak Evrensel kromm ahşap lateral veya Lateral katlanır korkuluklarla tam teşekküllü bir yatak haline getirebilirsiniz, 3 motorlu halinde mevcuttur. Avrupa standartlarına uygun ve CE markajlı karyolalar,Herdegen tarafında 15 yıldır evde hasta bakımı için üretilmektedir. Tüm yataklarımız Fransada imal ediliyor ve yüksek kaliteli Linak motorlar ile donatılmıştır. Karyolalarımız kolayca katlanır ve hasta evine teslim edilir.Makas mekanizması ile tasarlanmış ve 3 motorla donatılmıştır. Yatan hastanın kalkış ve yatışları düşünülerek karyolanın baş kısmında hasta tutamağı(deveboynu) tutunarak kalkışlarda ve yatışlarda hastanın kuvvet almasını sağlar.",
                            ProductDescriptionEN = $"Product-{i} Description: Our product, which is designed with wheels and steel body, can be adjusted including the head, backrest and foot part thanks to its control control. You can add a separate air to your bed with imitation beech, MDF or Wooden boards. In addition, you can turn Universal chrome wooden lateral or Lateral folding rails into a full-fledged bed, available in 3 motors. CE-marked cots complying with European standards have been produced by Herdegen for 15 years at home for patient care. All our bearings are manufactured in France and equipped with high quality Linak engines. Our beds are easily folded and delivered to the patient home. They are designed with a scissors mechanism and equipped with 3 motors. By considering the departure and hospitalization of the inpatient, the patient's handle (gooseneck) is held at the head of the bed, allowing the patient to gain strength during departures and hospitalizations.",
                            ProductDescriptionRU = $"продукт-{i} описание: Наш продукт, который разработан с колесами и стальным корпусом, может быть отрегулирован, включая голову, спинку и ножку, благодаря своему управлению. Вы можете добавить отдельный воздух к вашей кровати с имитацией бука, МДФ или деревянных досок. Кроме того, вы можете превратить универсальные хромированные деревянные боковые или боковые откидные рельсы в полноценную кровать, доступную в 3 моторах. Кровати с маркировкой CE, соответствующие европейским стандартам, производятся Herdegen уже 15 лет на дому для ухода за пациентами. Все наши подшипники производятся во Франции и оснащены высококачественными двигателями Linak. Наши кровати легко складываются и доставляются к пациенту домой, они оснащены ножничным механизмом и оснащены 3 моторами. Принимая во внимание отправление и госпитализацию стационарного больного, ручка пациента (гусиная шея) удерживается в верхней части кровати, что позволяет пациенту набирать силу во время отъезда и госпитализации.",
                            HeadTitleTR = "Türkçe meta title tag",
                            HeadTitleEN = "İngilizce meta title tag",
                            HeadTitleRU = "Rusça meta title tag",
                            HeadDescriptionTR = "Türkçe meta description tag",
                            HeadDescriptionEN = "İngilizce meta description tag",
                            HeadDescriptionRU = "Rusça meta description tag",
                            ProductOfferType = FakeData.EnumData.GetElement<ProductOfferType>(),
                            TopCategoryId = topCateId,
                            MiddleCategoryId = midCateId,
                            HasNewBadge = counter2 <= 3 ? true : false,
                            IsFreeShipping = counter3 <= 4 ? true : false,
                            DiscountRate = discountList[0],
                            ProductCode = rnd.Next(1000, 5000).ToString(),
                            StockNumber = rnd.Next(5000, 9999),
                            PhotoAltTagEN = "İngilizce Ürün Fotoğraf Alt Tagi",
                            PhotoAltTagTR = "Türkçe Ürün Fotoğraf Alt Tagi",
                            PhotoAltTagRU = "Rusça Ürün Fotoğraf Alt Tagi",
                        };
                        product.ProductNameUrlTR = product.NameTR.ConvertToFriendlyUrl();
                        product.ProductNameUrlEN = product.NameEN.ConvertToFriendlyUrl();
                        product.ProductNameUrlRU = product.NameEN.ConvertToFriendlyUrl();

                        _productRepo.CreateEntity(product);
                    }
                }
            }
        }

        public void PopulateProductMainPhotos()
        {
            string photoFolderPath = Path.Combine(_environment.WebRootPath, "MainYedek");
            string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Main");
            var files = Directory.GetFiles(photoFolderPath);
            files.ShuffleMyList();
             
            if (!_proPhotoRepo.AnyEntity())
            {
                for (int i = 1; i <= 500; i++)
                {
                    FileInfo fs = new FileInfo(files[0]);
                    string originalFileName = fs.Name;
                    string newFileName = GetUniqueFileName(originalFileName, i);

                    string fullPathModified = Path.Combine(folderPathModified, newFileName);
                     
                    var imageModified = new Bitmap(250, 250);
                    imageModified.Save(fullPathModified);
                    imageModified.Dispose();
                    string fullPathRaw = Path.Combine(photoFolderPath, originalFileName);
                    bool a = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 250, 250);

                    ProductPhoto photo = new ProductPhoto();
                    photo.ProductId = i;
                    photo.PhotoFileName = newFileName;
                    photo.IsMainPhoto = true;
                    _proPhotoRepo.CreateEntity(photo);

                    var productt = _productRepo.GetEntityById(i);
                    productt.MainPhotoFileName = newFileName;
                    _productRepo.UpdateEntity(productt);

                    files.ShuffleMyList();

                }
            }
        }

        private string GetUniqueFileName(string fileName, int productId)
        {
            var compId = _productRepo.GetProductWithId(productId).BrandId;
            return "ProId-" + productId + "_CompId-" + compId
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        public void PopulateProductAdditionalPhotos()
        {
            string photoFolderPath = Path.Combine(_environment.WebRootPath, "AdditionalYedek");
            string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Additional");
            string thumbFolderPath = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Thumb");
            var files = Directory.GetFiles(photoFolderPath);
            files.ShuffleMyList();

            for (int i = 0; i < 4; i++)
            {
                for (int ii = 1; ii <= 500; ii++)
                {
                    FileInfo fs = new FileInfo(files[0]);
                    string originalFileName = fs.Name;
                    string newFileName = GetUniqueFileName(originalFileName, ii);

                    string fullPathModified = Path.Combine(folderPathModified, newFileName);
                    string thumbFullPath = Path.Combine(thumbFolderPath, newFileName);

                    var imageModified = new Bitmap(500, 500);
                    imageModified.Save(fullPathModified);
                    imageModified.Dispose();

                    var imageModified2 = new Bitmap(85, 85);
                    imageModified2.Save(fullPathModified);
                    imageModified2.Dispose();

                    string fullPathRaw = Path.Combine(photoFolderPath, originalFileName);

                    bool a = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 500, 500);
                    bool b = ImageHelper.ResizeAndCompressImage(fullPathRaw, thumbFullPath, ImageFormat.Jpeg, 85, 85);

                    ProductPhoto photo = new ProductPhoto();
                    photo.ProductId = ii;
                    photo.PhotoFileName = newFileName;
                    photo.IsMainPhoto = false;
                    _proPhotoRepo.CreateEntity(photo);

                    files.ShuffleMyList();
                }
            }
        }

        public void PopulateCustomers()
        {
            if (!_customerRepo.AnyEntity())
            {
                for (int i = 1; i <= 100; i++)
                {
                    _customerRepo.CreateEntity(new Customer
                    {
                        CellPhoneNumber = FakeData.PhoneNumberData.GetInternationalPhoneNumber(),
                        Password = "11111",
                        EmailAddress = FakeData.NetworkData.GetEmail(),
                        IsSubscribedToEmail = FakeData.BooleanData.GetBoolean(),
                        IsSubscribedToSMS = FakeData.BooleanData.GetBoolean(),
                        NameSurname = FakeData.NameData.GetFullName(),
                        Address=FakeData.PlaceData.GetAddress()
                    });
                }
            }
        }

        public void PopulateFavoriteProducts()
        {
            if (!_fpRepo.AnyEntity())
            {
                Random rnd = new Random();
                int proId;
                int cusId;
                for (int i = 1; i <= 200; i++)
                {
                    FavoriteProduct pro = new FavoriteProduct();
                    do
                    {
                        proId = rnd.Next(1, 501);
                        cusId = rnd.Next(1, 100);

                    } while (_fpRepo.AnyEntity(x => x.CustomerId == cusId && x.ProductId == proId));
                    pro.ProductId = proId;
                    pro.CustomerId = cusId;
                    _fpRepo.CreateEntity(pro);
                }
            }
        }

        public void PopulateAdmins()
        {
            if (!_adminRepo.AnyEntity())
            {
                for (int i = 1; i < 20; i++) 
                {
                    _adminRepo.CreateEntity(new Admin
                    {
                        CellPhoneNumber = FakeData.PhoneNumberData.GetInternationalPhoneNumber(),
                        Password = "11111",
                        EmailAddress = FakeData.NetworkData.GetEmail(),
                        NameSurname = FakeData.NameData.GetFullName()
                    });
                }
            }
        }

        public int MiddleCategoryFinder(int topCateId)
        {
            var midCates = _midCateRepo.FindEntities(x => x.TopCategoryId == topCateId).ToList();
            var list = new List<int>();
            foreach (var item in midCates)
            {
                list.Add(item.Id);
            }
            list.ShuffleMyList();
            return list[0];
        }

        public int SubCategoryFinder(int middleCateId)
        {
            var subCates = _subCateRepo.FindEntities(x => x.MiddleCategoryId == middleCateId).ToList();
            var list = new List<int>();
            foreach (var item in subCates)
            {
                list.Add(item.Id);
            }
            list.ShuffleMyList();
            return list.ElementAt(0);
        }
    }
}