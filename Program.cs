using System;
using System.Data;

namespace KITAP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] barkod = new int[100];
            string[] kitap = new string[100];
            string[] yazar = new string[100];
            int[] stok = new int[100];
            int[] yil = new int[100];

            string[] kiralananKitaplar = new string[100];
            int[] kiralananAdet = new int[100];
            int[] kiralamaGun = new int[100];

            int kiralananIndex = 0;
            // pm başlangıç ürün miktarı , p ürün stok artış miktarı
            int pm = 0;
            int p = 1;


            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("İşlem Menüsüne Hoşgeldiniz");
                Console.WriteLine();
                Console.WriteLine("1-Kitap Eklemek İçin ");
                Console.WriteLine("2-Kitap Kiralamak İçin ");
                Console.WriteLine("3-Kitap iade İçin ");
                Console.WriteLine("4-Kitap Listesi İçin ");
                Console.WriteLine("5-Kiralanan Kitaplar");

                string sec = Console.ReadLine();

                if (sec == "1")
                {
                    Console.Write("Kitap ismi: ");
                    kitap[pm] = Console.ReadLine();
                    Console.Write("Kitap Yazarı: ");
                    yazar[pm] = Console.ReadLine();
                    Console.Write("Kitap Yayın Yılı: ");
                    yil[pm] = int.Parse(Console.ReadLine());
                    Console.Write("Stok miktarı: ");
                    stok[pm] = int.Parse(Console.ReadLine());

                    bool kitapVarMi = false;
                    for (int i = 0; i < pm; i++)
                    {

                        if (kitap[i] == kitap[pm] && yazar[i] == yazar[pm] && yil[i] == yil[pm])
                        {
                            kitapVarMi = true;
                            stok[i] += stok[pm];

                            Console.WriteLine($"{kitap[i]} Zaten Mevcut. Stok artırıldı. Yeni stok: {stok[i]}");
                            break;
                        }
                        else
                        {

                            Console.WriteLine("Kitap Eklendi!");
                        }

                    }
                    if (kitapVarMi == false)
                    {
                        barkod[pm] = p++;
                        pm++;
                    }
                }

                else if (sec == "2")
                {
                    Console.WriteLine("\n****Kiralık Kitap Listesi ****");
                    for (int i = 0; i < pm; i++)
                    {
                        Console.WriteLine($"Barkod: {barkod[i]}, İsim: {kitap[i]}, Fiyat: {yazar[i]:C}, Yayın Yılı: {yil[i]}, Stok: {stok[i]}");
                    }

                    Console.Write("Almak istediğiniz Kitap barkodunu girin: ");
                    int albarkod = int.Parse(Console.ReadLine());
                    Console.Write("Kaç adet almak istiyorsunuz? ");
                    int alAdet = int.Parse(Console.ReadLine());

                    bool kitapBulundu = false;
                    for (int i = 0; i < pm; i++)
                    {
                        if (barkod[i] == albarkod)
                        {
                            kitapBulundu = true;
                            if (stok[i] >= alAdet)
                            {
                                Console.Write("Kiralamak istediğiniz tarihi giriniz (GG/AA/YYYY formatında): ");
                                string tarihInput = Console.ReadLine();

                                DateTime kiralamaTarihi;
                                if (DateTime.TryParse(tarihInput, out kiralamaTarihi))
                                {
                                    DateTime bugun = DateTime.Now;

                                    int gunSayisi = (kiralamaTarihi - bugun).Days;

                                    if (gunSayisi > 0)
                                    {
                                        Console.Write("Bütçenizi girin: ");
                                        double butce = double.Parse(Console.ReadLine());
                                        int fiyat = 5;
                                        if (butce >= gunSayisi * alAdet * fiyat)
                                        {
                                            Console.WriteLine($"Kitap {gunSayisi} gün kiralandı: Tutar: {gunSayisi * alAdet * fiyat}, Kalan Tutar: {butce - (fiyat * alAdet * gunSayisi)}");
                                            stok[i] -= alAdet;
                                            kiralananKitaplar[kiralananIndex] = kitap[i];
                                            kiralananAdet[kiralananIndex] = alAdet;
                                            kiralamaGun[kiralananIndex] = gunSayisi;
                                            kiralananIndex++;
                                            Console.WriteLine($"{alAdet} adet {kitap[i]} başarıyla kiralandı. Yeni stok: {stok[i]}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Bütçeniz yeterli değil.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Girdiğiniz tarih bugünden sonraki bir tarih olmalıdır.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Geçersiz tarih formatı.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Yeterli stok bulunmamaktadır!");
                            }
                            break;
                        }
                    }

                    if (!kitapBulundu)
                    {
                        Console.WriteLine("Girilen barkod ile eşleşen bir kitap bulunamadı.");
                    }
                }

                else if (sec == "3")
                {
                    Console.WriteLine("\n****Kiralanan Kitaplar****");
                    for (int i = 0; i < kiralananIndex; i++)
                    {
                        Console.WriteLine($"Barkod: {i + 1}, Kitap: {kiralananKitaplar[i]}, Adet: {kiralananAdet[i]}, Gün: {kiralamaGun[i]}");
                    }

                    Console.Write("İade etmek istediğiniz kitabın Barkodu girin: ");
                    int iadebarkod = int.Parse(Console.ReadLine()) - 1;

                    if (iadebarkod >= 0 && iadebarkod < kiralananIndex)
                    {

                        for (int i = 0; i < pm; i++)
                        {
                            if (kiralananKitaplar[i] == kitap[i])
                            {
                                stok[i] += kiralananAdet[iadebarkod];
                                Console.WriteLine($"{kiralananAdet[i]} adet {kitap[i]} başarıyla iade edildi. Yeni stok: {stok[i]}");
                                break;
                            }
                        }
                        for (int i = iadebarkod; i < kiralananIndex - 1; i++)
                        {
                            kiralananKitaplar[i] = kiralananKitaplar[i + 1];
                            kiralananAdet[i] = kiralananAdet[i + 1];
                            kiralamaGun[i] = kiralamaGun[i + 1];
                        }

                        kiralananIndex--;
                        Console.WriteLine("Kitap iade edildi ve listeden çıkarıldı.");
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz barkod.");
                    }
                }

                else if (sec == "4")
                {
                    Console.WriteLine("\n****Kitap Listesi****");
                    for (int i = 0; i < pm; i++)
                    {

                        Console.WriteLine($"Barkod: {barkod[i]}, Kitap: {kitap[i]}, Yazar: {yazar[i]}, Yayın Yılı:{yil[i]} Stok: {stok[i]}");
                    }
                }
                else if (sec == "5")
                {
                    Console.WriteLine("\n****Kiralanan Kitaplar****");
                    if (kiralananIndex == 0)
                    {
                        Console.WriteLine("Şu anda kiralanan kitap bulunmamaktadır.");
                    }
                    else
                    {
                        for (int i = 0; i < kiralananIndex; i++)
                        {
                            Console.WriteLine($"Barkod: {i}, Kitap: {kiralananKitaplar[i]}, Adet: {kiralananAdet[i]}, Kiralama Süresi: {kiralamaGun[i]} gün");
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                }
            }
        }
    }
}


