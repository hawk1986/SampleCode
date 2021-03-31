using MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json;
using ResourceLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using SampleCode.Models;

namespace SampleCode.ViewModel
{
    public class PostedFileBaseModel
    {
        /// <summary>
        /// 建構函數
        /// </summary>
        public PostedFileBaseModel()
        {
            AttachedFiles = new List<FileAttached>();
            NewAttachedFiles = new List<FileAttached>();
            DeleteFileID = new List<Guid>();
            ReservedID = new List<Guid>();
            PostedCategory = new List<string>();
            PostedCaption = new List<string>();

            #region Banner
            AttachedBigPic = new List<FileAttached>();
            PostedBigPicFiles = new List<HttpPostedFileBase>();
            PostedBigPicCaption = new List<string>();
            AttachedSmallPic = new List<FileAttached>();
            PostedSmallPicFiles = new List<HttpPostedFileBase>();
            PostedSmallPicCaption = new List<string>();
            #endregion

            #region DM
            AttachedEBooks = new List<FileAttached>();
            PostedEBookFiles = new List<HttpPostedFileBase>();
            PostedEBookCaption = new List<string>();
            #endregion

            #region SpaceManagement
            AttachedDetail = new List<FileAttached>();
            PostedDetailFiles = new List<HttpPostedFileBase>();
            PostedDetailCaption = new List<string>();
            PicOrder = new List<int?>();
            MainPic = new List<bool>();
            PostedCaption1 = new List<string>();
            PicOrder1 = new List<int?>();
            MainPic1 = new List<bool>();
            IsTrue = new List<string>();
            #endregion

            #region FloorManagement
            AttachedHoleArea = new List<FileAttached>();
            PostedHoleAreaFiles = new List<HttpPostedFileBase>();
            PostedHoleAreaCaption = new List<string>();
            AttachedArea = new List<FileAttached>();
            PostedAreaFiles = new List<HttpPostedFileBase>();
            PostedAreaCaption = new List<string>();
            PostAreaKind = new List<string>();
            AreaReservedID = new List<Guid>();
            #endregion

            #region BrandManagement
            AttachedQRCode = new List<FileAttached>();
            PostedQRCodeFiles = new List<HttpPostedFileBase>();
            PostedQRCodeCaption = new List<string>();
            #endregion

            #region Trend
            BuyGuideReservedID = new List<Guid>();
            AddAttachedBuyGuide = new List<FileAttached>();
            AttachedBuyGuide = new List<FileAttached>();
            PostedBuyGuideFiles = new List<HttpPostedFileBase>();
            PostedBuyGuideCaption = new List<string>();
            #endregion

            #region ServiceManagement
            AttachedMaps = new List<FileAttached>();
            PostedMapFiles = new List<HttpPostedFileBase>();
            PostedMapCaption = new List<string>();
            #endregion

            #region VideoManagement
            AttachedVideo2 = new List<FileAttached>();
            PostedVideo2Files = new List<HttpPostedFileBase>();
            PostedVideo2Caption = new List<string>();
            #endregion

            #region ActivityManagement
            AttachedCoupon = new List<FileAttached>();
            PostedCouponFiles = new List<HttpPostedFileBase>();
            PostedCouponCaption = new List<string>();

            AttachedAndroidPic = new List<FileAttached>();
            PostedAndroidPicFiles = new List<HttpPostedFileBase>();
            PostedAndroidPicCaption = new List<string>();

            AttachedAndroidQR = new List<FileAttached>();
            PostedAndroidQRFiles = new List<HttpPostedFileBase>();
            PostedAndroidQRCaption = new List<string>();

            AttachedApplePic = new List<FileAttached>();
            PostedApplePicFiles = new List<HttpPostedFileBase>();
            PostedApplePicCaption = new List<string>();

            AttachedAppleQR = new List<FileAttached>();
            PostedAppleQRFiles = new List<HttpPostedFileBase>();
            PostedAppleQRCaption = new List<string>();
            #endregion
        }

        /// <summary>
        /// 附加檔案
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedFiles { get; set; }

        /// <summary>
        /// 新增的附加檔案
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> NewAttachedFiles { get; set; }

        /// <summary>
        /// 刪除的檔案路徑清單
        /// </summary>
        [JsonIgnore]
        public List<Guid> DeleteFileID { get; set; }

        /// <summary>
        /// 保留的檔案編號
        /// </summary>
        [JsonIgnore]
        public List<Guid> ReservedID { get; set; }

        /// <summary>
        /// 上傳檔案的分類
        /// </summary>
        [JsonIgnore]
        public List<string> PostedCategory { get; set; }

        /// <summary>
        /// 上傳檔案的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedCaption { get; set; }

        /// <summary>
        /// 上傳檔案清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedFiles { get; set; }

        /// <summary>
        /// 上傳檔案清單
        /// </summary>
        [JsonIgnore]
        public List<HttpFile> HttpFiles { get; set; }

        #region Banner
        /// <summary>
        /// 附加圖檔(個人圖片)
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedBigPic { get; set; }

        /// <summary>
        /// 上傳大圖檔清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedBigPicFiles { get; set; }

        /// <summary>
        /// 上傳大圖檔的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedBigPicCaption { get; set; }

        /// <summary>
        /// 附加圖檔(個人圖片)
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedSmallPic { get; set; }

        /// <summary>
        /// 上傳小圖檔清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedSmallPicFiles { get; set; }

        /// <summary>
        /// 上傳小圖檔的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedSmallPicCaption { get; set; }
        #endregion

        #region DM
        /// <summary>
        /// 附加電子書
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedEBooks { get; set; }

        /// <summary>
        /// 上傳電子書清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedEBookFiles { get; set; }

        /// <summary>
        /// 上傳電子書的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedEBookCaption { get; set; }
        #endregion

        #region SpaceManagement
        /// <summary>
        /// 場地詳細資料
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedDetail { get; set; }

        /// <summary>
        /// 場地詳細資料清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedDetailFiles { get; set; }

        /// <summary>
        /// 場地詳細資料說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedDetailCaption { get; set; }
        [JsonIgnore]
        public List<int?> PicOrder { get; set; }
        [JsonIgnore]
        public List<bool> MainPic { get; set; }
        [JsonIgnore]
        public List<string> PostedCaption1 { get; set; }
        [JsonIgnore]
        public List<int?> PicOrder1 { get; set; }
        [JsonIgnore]
        public List<bool> MainPic1 { get; set; }
        [JsonIgnore]
        public List<string> IsTrue { get; set; }
        #endregion

        #region FloorManagement
        /// <summary>
        /// 場地詳細資料
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedArea { get; set; }

        /// <summary>
        /// 場地詳細資料清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedAreaFiles { get; set; }

        /// <summary>
        /// 場地詳細資料說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedAreaCaption { get; set; }

        /// <summary>
        /// 全區圖詳細資料
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedHoleArea { get; set; }

        /// <summary>
        /// 全區圖詳細資料清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedHoleAreaFiles { get; set; }

        /// <summary>
        /// 全區圖詳細資料說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedHoleAreaCaption { get; set; }
        [JsonIgnore]
        public List<string> PostAreaKind { get; set; }

        /// <summary>
        /// 保留的檔案編號
        /// </summary>
        [JsonIgnore]
        public List<Guid> AreaReservedID { get; set; }
        #endregion

        #region BrandManagement
        /// <summary>
        /// 附加QRCode
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedQRCode { get; set; }

        /// <summary>
        /// 上傳QRCode清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedQRCodeFiles { get; set; }

        /// <summary>
        /// 上傳QRCode的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedQRCodeCaption { get; set; }
        #endregion

        #region Trend
        /// <summary>
        /// 保留的BuyGuide編號
        /// </summary>
        [JsonIgnore]
        public List<Guid> BuyGuideReservedID { get; set; }

        /// <summary>
        /// 導購資訊(新增用)
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AddAttachedBuyGuide { get; set; }

        /// <summary>
        /// 導購資訊
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedBuyGuide { get; set; }

        /// <summary>
        /// 上傳QRCode清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedBuyGuideFiles { get; set; }

        /// <summary>
        /// 上傳QRCode的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedBuyGuideCaption { get; set; }
        #endregion

        #region ServiceManagement
        /// <summary>
        /// 附加電子書
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedMaps { get; set; }

        /// <summary>
        /// 上傳電子書清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedMapFiles { get; set; }

        /// <summary>
        /// 上傳電子書的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedMapCaption { get; set; }
        #endregion

        #region VideoManagement
        /// <summary>
        /// 附加橫向影片
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedVideo2 { get; set; }

        /// <summary>
        /// 上傳橫向影片清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedVideo2Files { get; set; }

        /// <summary>
        /// 上傳橫向影片的說明
        /// </summary>
        [JsonIgnore]
        public List<string> PostedVideo2Caption { get; set; }
        #endregion

        #region ActivityManagement
        /// <summary>
        /// 附加Coupon
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedCoupon { get; set; }

        /// <summary>
        /// 上傳Coupon清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedCouponFiles { get; set; }

        /// <summary>
        /// 上傳Coupon
        /// </summary>
        [JsonIgnore]
        public List<string> PostedCouponCaption { get; set; }

        /// <summary>
        /// 附加AndroidPic
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedAndroidPic { get; set; }

        /// <summary>
        /// 上傳AndroidPic清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedAndroidPicFiles { get; set; }

        /// <summary>
        /// 上傳AndroidPic
        /// </summary>
        [JsonIgnore]
        public List<string> PostedAndroidPicCaption { get; set; }

        /// <summary>
        /// 附加AndroidQR
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedAndroidQR { get; set; }

        /// <summary>
        /// 上傳AndroidQR清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedAndroidQRFiles { get; set; }

        /// <summary>
        /// 上傳AndroidQR
        /// </summary>
        [JsonIgnore]
        public List<string> PostedAndroidQRCaption { get; set; }

        /// <summary>
        /// 附加ApplePic
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedApplePic { get; set; }

        /// <summary>
        /// 上傳ApplePic清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedApplePicFiles { get; set; }

        /// <summary>
        /// 上傳ApplePic
        /// </summary>
        [JsonIgnore]
        public List<string> PostedApplePicCaption { get; set; }

        /// <summary>
        /// 附加AppleQR
        /// </summary>
        [JsonIgnore]
        public List<FileAttached> AttachedAppleQR { get; set; }

        /// <summary>
        /// 上傳AppleQR清單
        /// </summary>
        [JsonIgnore]
        [Display(Name = "UploadFile", ResourceType = typeof(Resource))]
        public List<HttpPostedFileBase> PostedAppleQRFiles { get; set; }

        /// <summary>
        /// 上傳AppleQR
        /// </summary>
        [JsonIgnore]
        public List<string> PostedAppleQRCaption { get; set; }
        #endregion
    }
}