using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterAndWastewaterAuthorithy.Presentation
{
    public static class Messages
    {
        /*        public static string ValueTooLong = "اندازه اطلاعات وارد شده صحیح نمی باشد.";
                public static string ForceToInputName = "لطفا نام را مشخص نمایید.";
                public static string ForceToInputFamily = "لطفا نام خانوادگی را مشخص نمایید.";
                public static string ForceToInputFather = "لطفا نام پدر را مشخص نمایید.";
                public static string ForceToInputAddress = "لطفا آدرس را مشخص نمایید.";
                public static string ForceToInputCellPhone = "لطفا تلفن همراه را مشخص نمایید.";*/

        public static string ErrorSendingDataToDatabase = "امکان ارتباط با پایگاه داده وجود ندارد.";

        #region Splash
        public static string SplashLoading = "نرم افزار اداره آب و فاضلاب";        
        #endregion
        
        #region MainMenu
        public static string MainMenuYearNotSelected = "سال مالی تعریف یا انتخاب نشده است. لطفا در ابتدا سال مالی را مشخص نمایید.";
        #endregion
        
        #region Customers
        public static string SaveMessageCustomers = "آیا مشخصات مشترک ثبت شود؟";
        public static string SaveMessageTitleCustomers = "ثبت مشخصات مشترک";
        public static string SaveMessageSuccessCustomers = "مشخصات مشترک با موفقیت ثبت شد.";
        public static string DeleteMessageTitleCustomers = "حذف مشخصات مشترک";
        public static string PrintMessageTitleCustomers = "چاپ مشخصات مشترک";
        public static string DeleteMessageCustomers = "آیا مشخصات مشترک حذف شود؟";
        public static string DeleteMessageSuccessCustomers = "مشخصات مشترک با موفقیت حذف شد.";
        public static string EditMessageTitleCustomers = "ویرایش مشخصات مشترک";
        public static string EditMessageCustomers = "آیا مشخصات مشترک ویرایش شود؟";
        public static string EditMessageSuccessCustomers = "مشخصات مشترک با موفقیت ویرایش شد.";
        public static string NotFoundCustomers = "مشخصات مشترک پیدا نشد.";
        public static string ForceToCustomersId = "ورود کد مشترک اجباری است.";
        public static string SearchMessageTitleCustomers = "جستجوی مشترک";
        public static string DeleteCustomerImpossible = "امکان حذف مشترک وجود ندارد. ابتدا اشتراک مربوطه را حذف نمایید.";
        public static string CustomerNotFound = "کاربر مورد نظر در سیستم وجود ندارد.";
        public static string MeliCodeIsRepetitive = "کد ملی وارد شده قبلا ثبت شده است.";
        public static string DataNotFoundForPrint = "اطلاعاتی جهت چاپ وجود ندارد.";
        #endregion

        #region Subscriptions
        public static string SaveMessageSubscriptions = "آیا مشخصات اشتراک ثبت شود؟";
        public static string SaveMessageTitleSubscriptions = "ثبت مشخصات اشتراک";
        public static string SaveMessageSuccessSubscriptions = "مشخصات اشتراک با موفقیت ثبت شد.";        
        public static string DeleteMessageTitleSubscriptions = "حذف مشخصات اشتراک";
        public static string DeleteMessageSubscriptions = "آیا مشخصات اشتراک حذف شود؟";
        public static string DeleteMessageSuccessSubscriptions = "مشخصات اشتراک با موفقیت حذف شد.";
        public static string EditMessageTitleSubscriptions = "ویرایش مشخصات اشتراک";
        public static string EditMessageSubscriptions = "آیا مشخصات اشتراک ویرایش شود؟";
        public static string EditMessageSuccessSubscriptions = "مشخصات اشتراک با موفقیت ویرایش شد.";
        public static string SubScriptInUseEdit = "برای اشتراک مورد نظر قبض صادر شده است، امکان ویرایش قرائت و تاریخ قرائت آن وجود ندارد.";
        public static string SubScriptInUseDelete = "برای اشتراک مورد نظر قبض صادر شده است، امکان حذف آن وجود ندارد.";
        public static string NotWaterMeterExist = "کنتور آب برای اشتراک تعریف شده است.";
        public static string PrintMessageTitleSubScription = "چاپ مشخصات اشتراک";
        public static string PostalCodeIsRepetitive = "کد پستی وارد شده قبلا ثبت شده است.";
        #endregion
        public static string PrintMessageReports = "چاپ گزارشات";
        #region BillPeriod
        public static string SaveMessageBillPeriods = "آیا مشخصات دوره صدور قبض ثبت شود؟";
        public static string SaveMessageTitleBillPeriods = "ثبت مشخصات دوره صدور قبض";
        public static string SaveMessageSuccessBillPeriods = "مشخصات دوره صدور قبض با موفقیت ثبت شد.";
        public static string DeleteMessageTitleBillPeriods = "حذف مشخصات دوره صدور قبض";
        public static string DeleteMessageBillPeriods = "آیا مشخصات دوره صدور قبض حذف شود؟";
        public static string DeleteMessageSuccessBillPeriods = "مشخصات دوره صدور قبض با موفقیت حذف شد.";
        public static string EditMessageTitleBillPeriods = "ویرایش مشخصات دوره صدور قبض";
        public static string EditMessageBillPeriods = "آیا مشخصات دوره صدور قبض ویرایش شود؟";
        public static string EditMessageSuccessBillPeriods = "مشخصات دوره صدور قبض با موفقیت ویرایش شد.";        
        public static string DeletePeriodIsClosedBillPeriods = "دوره صدور قبض بسته شده است امکان حذف آن وجود ندارد.";
        public static string MonthsIs1     = "برای از تاریخ بایستی اولین روز ماه انتخاب شود.";
        public static string MonthsIs31    = "آخرین روز ماه برای تا تاریخ 31 می باشد.";
        public static string MonthsIs30    = "آخرین روز ماه برای تا تاریخ 30 می باشد.";
        public static string MonthsIs29NotLeap    = "آخرین روز ماه اسفند برای تا تاریخ 29 می باشد.";
        public static string MonthsIs30Leap       = "آخرین روز ماه اسفند برای تا تاریخ 30 می باشد.";
        public static string PeriodInUseEdit = "امکان ویرایش تاریخ های دوره وجود ندارد لطفا دوره مورد نظر و همچنین دوره های بعدی آن را پاک کرده سپس دوره ها را مجددا تعریف نمایید.";
        public static string PeriodInUseDelete = "برای دوره مورد نظر قبض صادر شده است، امکان حذف آن وجود ندارد.";
        public static string PeriodNotInRange = "عدد وارد شده خارج از محدوده قابل تعریف دوره می باشد.";
        public static string IsNotPossibeToSelect = "امکان انتخاب دوره به عنوان دوره فعلی وجود ندارد.";
        public static string CloseMessageTitleBillPeriods = "بستن دوره صدور قبض";
        public static string ClosedMessageSuccessed = "دوره مورد نظر با موفقیت بسته شد.";
        public static string ImpossibleClosedMessage = "امکان بستن دوره مورد نظر وجود ندارد لطفا دوره را به عنوان دوره جاری انتخاب نمایید.";
        public static string PrevRowExits = "امکان حذف دوره وجود ندارد ابتدا دوره های بعدی را حذف نموده سپس اقدام به حذف دوره مورد نظر نمایید.";
        public static string PeriodsIsClosed = "آیا دوره مورد نظر بسته شود؟";
        public static string PeriodClosed = "دوره مورد نظر بسته شده است.";
        #endregion

        #region Billing
        public static string NotExistPeriodBilling = "دوره ای برای سال جاری تعریف یا انتخاب نشده است.";
        public static string PeriodIsClosedBilling = "دوره انتخاب شده بسته شده است امکان صدور قبض جدید در دوره وجود ندارد.";
        public static string PeriodIsClosedBillingReceivable = "دوره انتخاب شده بسته شده است امکان وصول قبض در دوره وجود ندارد.";
        public static string PeriodIsClosedBillingCancel = "دوره انتخاب شده بسته شده است امکان ابطال قبض در دوره وجود ندارد.";
        public static string SaveMessageBilling = "آیا قبوض اشتراک ها ثبت شود؟";
        public static string PreSaveMessageSuccessBilling = "قبض اشتراک با موفقیت ثبت موقت شد.";
        public static string SaveMessageBillingNext = "آیا مطمئن هستید که قبض برای اشتراک مورد نظر ثبت نشود؟";
        public static string SaveMessageTitleBilling = "ثبت قبوض دوره ای";
        public static string SaveMessageSuccessBilling = "قبوض اشتراک ها با موفقیت ثبت شد.";
        public static string NotExistSubscription = "امکان صدور قبض برای اشتراک مورد نظر وجود ندارد.";
        public static string NumberIsWrong = "قرائت فعلی وارد شده درست نمی باشد.";
        public static string DateIsWrong = "تاریخ قرائت فعلی وارد شده درست نمی باشد.";
        public static string ToDateIsWrong = "تاریخ قرائت فعلی بایستی بعد از تاریخ قرائت قبلی است.";
        public static string FormulaIsWrong = "فرمول وارد شده برای نوع کاربری اشتباه می باشد.";
        public static string SubScriptionIsWrong = "امکان بارگذاری اشتراک مورد نظر وجود ندارد. لطفا مشخصات اشتراک مورد نظر را بررسی نمایید.";
        public static string SubScriptionNextNotFound = "تمامی اشتراک های دوره ثبت شده است.";
        public static string SubScriptionNotFound = "اشتراکی برای صدور قبض وجود ندارد.";
        public static string WaitingLoadSub = "اطلاعات اشتراک در حال بارگذاری است لطفا صبر نمایید...";
        public static string LoadSubTitle = "صدور قبض دوره ای";        
        #endregion

        #region SingleBilling
        public static string SaveMessageSingleBilling = "آیا قبض اشتراک ثبت شود؟";        
        public static string SaveMessageTitleSingleBilling = "ثبت قبوض تکی دوره";
        public static string SaveMessageTitleWaterMeter = "ثبت کنتور آب جدید";
        public static string ForceToSubscriptionId = "ورود کد اشتراک اجباری است.";
        public static string NotFoundSubscription = "مشخصات اشتراک پیدا نشد.";
        public static string SaveMessageSuccessSingleBilling = "قبض اشتراک با موفقیت ثبت شد.";
        public static string WaterMeterIsRegistered = "کنتور آب جدید مورد نظر با موفقیت ثبت شد";
        public static string WaterMeterAndBillIsRegistered = "آخرین قبض این اشتراک برای کنتور قدیمی ثبت و کنتور آب جدید مورد نظر با موفقیت ثبت شد";

        public static string DateIsNotAllowedBiggarThanCurrentDate = "تاریخ قرائت نمی تواند پس از تاریخ روز باشد.";
        public static string WaterMeterDateIsNotAllowedBiggarThanCurrentDate = "تاریخ قرائت کنتور جدید نمی تواند پس از تاریخ روز باشد.";
        public static string PrintMessageTitleSingleBilling = "چاپ قبوض";
        public static string WaterMeterDateIsNotAllowedBiggarThanLastDate = "تاریخ قرائت کنتور جدید نمی تواند قبل از تاریخ آخرین قرائت باشد.";
        #endregion

        public static string SaveMessageNewWaterMeter = "آیا مطمئن هستید کنتور جدید ثبت شود؟";

        #region BillsReceivable
        public static string SaveMessageBillsReceivable = "آیا وصول قبض اشتراک ثبت شود؟";
        public static string SaveMessageTitleBillsReceivable = "ثبت وصول آخرین قبض اشتراک دوره";
        public static string SaveMessageSuccessBillsReceivable = "وصول قبض اشتراک با موفقیت ثبت شد.";
        public static string PriceIsNotAllowedZeroOrLess = "مبلغ وارده شده نمی تواند صفر یا کمتر از صفر باشد.";
        public static string CurrentDateIsNotAllowedLowerThanCurrentReadDate = "تاریخ وصول نمی تواند کمتر از تاریخ قرائت فعلی باشد.";
        public static string PriceIsNotEqualWithDebt = "مبلغ وارد شده با مبلغ قابل دریافت منطبق نمی باشد، آیا مطمئن به ادامه عملیات هستید؟";
        public static string DateIsNotAllowedBiggarThanReceivableDate = "تاریخ وصول نمی تواند پس از تاریخ روز باشد.";
        public static string NotFoundSubscriptionOrBill = "قبض یا اشتراکی برای وصول وجود ندارد.";
        public static string PrintMessageTitleBillsReceivable = "چاپ قبوض وصول شده";
        #endregion

        #region BillsCancelling
        public static string SaveMessageBillsCancelling = "آیا ابطال قبض اشتراک ثبت شود؟";
        public static string SaveMessageTitleBillsCancelling = "ابطال آخرین قبض اشتراک در دوره";
        public static string SaveMessageSuccessBillsCancelling = "ابطال قبض اشتراک با موفقیت ثبت شد.";                       
        //public static string NotFoundSubscriptionOrBillForCanceeling = "قبض یا اشتراکی برای ابطال وجود ندارد.";
        public static string NotFoundSubscriptionOrBillForCancelling = "قبض یا اشتراکی برای ابطال وجود ندارد.";
        public static string CanNotCancelBillBecauseOfWaterMeter = "قبض مورد نظر مربوط به کنتور فعلی نمی باشد. به همین دلیل امکان ابطال قبض مورد نظر وجود ندارد";
        #endregion

        #region Settings
        public static string SaveMessageSettings = "آیا تنظیمات سیستم ثبت شود؟";
        public static string SaveMessageTitleSettings = "ثبت تنظیمات سیستم";
        public static string SaveMessageSuccessSettings = "تنظیمات سیستم با موفقیت ثبت شد.";
        public static string CurrentYearConfilctWithThisYearSettings = "آیا مطمئن هستید که می خواهید سال مالی را تغییر دهید؟";
        public static string CreateYearImpossible = "برای ایجاد سال مالی جدید ابتدا سال مالی قبلی را ببندبد.";
        public static string CreateYearTitleSettings = "ایجاد سال مالی";
        public static string CreateYearMessageSuccessSettings = "سال مالی جدید با موفقیت ایجاد شد.";
        public static string CloseYearTitleSettings = "بستن سال مالی";
        public static string IsCloseYear = "آیا مطمئن هستید سال مالی جاری بسته شود.";
        public static string YearNotSelected = "لطفا سال مالی مورد نظر را انتخاب نمایید.";
        public static string CloseYearImpossible = "امکان بستن سال مالی وجود ندارد.";
        public static string CloseYearMessageSuccess = "سال مالی مورد نظر با موفقیت بسته شد.";
        #endregion

        #region AccountTypes
        public static string SaveMessageAccountTypes = "آیا مشخصات نوع کاربری ثبت شود؟";
        public static string SaveMessageTitleAccountTypes = "ثبت مشخصات نوع کاربری";
        public static string SaveMessageSuccessAccountTypes = "مشخصات نوع کاربری با موفقیت ثبت شد.";
        public static string DeleteMessageTitleAccountTypes = "حذف مشخصات نوع کاربری";
        public static string DeleteMessageAccountTypes = "آیا مشخصات مشترک حذف شود؟";
        public static string DeleteMessageSuccessAccountTypes = "مشخصات نوع کاربری با موفقیت حذف شد.";
        public static string EditMessageTitleAccountTypes = "ویرایش مشخصات نوع کاربری";
        public static string EditMessageAccountTypes = "آیا مشخصات نوع کاربری ویرایش شود؟";
        public static string EditMessageSuccessAccountTypes = "مشخصات نوع کاربری با موفقیت ویرایش شد.";
        public static string FormuleIsNotValid = "فرمول نوع کاربری وارد شده صحیح نمی باشد.";
        public static string NotFoundAccountTypes = "مشخصات نوع کاربری پیدا نشد.";
        public static string DeleteAccountTypeImpossible = "امکان حذف نوع کاربری وجود ندارد. ابتدا اشتراک مربوطه را حذف نمایید.";
        public static string PrintMessageTitleAccountTypes = "چاپ مشخصات نوع کاربری";
        #endregion

        #region PreventTypes
        public static string NotFoundPreventTypes = "مشخصات کد مانع پیدا نشد.";
        #endregion

        #region Reports
        public static string ReportTitle = "گزارش از لیست ";
        public static string ReportTitleDialog = "گزارشات";
        public static string ReportError = "امکان گزارش گیری از لیست مورد نظر وجود ندارد.";
        #endregion

        #region RepotSubAllPeriods
        public static string SubIdIsMandatory = "لطفا شماره اشتراک مورد نظر را وارد نمایید";
        #endregion

        public static string ForceToInputStart = "ورود ";
        public static string ForceToInputEnd = " اجباری است لطفا مقدار آن را مشخص نمایید.";
        public static string ValueStringTooLongStart = "اندازه ";
        public static string ValueStringTooLongMiddle = " وارد شده بیش از ";
        public static string ValueStringTooLongEnd = " حرف می باشد.";
        public static string ValueNumberTooLongStart = "اندازه ";
        public static string ValueNumberTooLongMiddle = " نمی تواند بیش از ";
        public static string ValueNumberTooLongEnd = " رقم باشد.";
        public static string ValueIsNumberStart = "لطفا مقدار ";
        public static string ValueIsNumberEnd = " را به صورت عددی وارد نمایید.";
        public static string ValueNumberTooShortStart = "اندازه ";
        public static string ValueNumberTooShortMiddle = " نمی تواند کمتر از ";
        public static string ValueNumberTooShortEnd = " رقم باشد.";
        public static string ValueIsSubscriptionNumberStart = "لطفا مقدار ";
        public static string ValueIsSubscriptionNumberEnd = " را به صورت (عدد-عدد) وارد نمایید.";
        public static string ForceToInputComboStart = "ورود ";
        public static string ForceToInputComboEnd = " اجباری است لطفا گزینه مورد نظر را انتخاب نمایید.";
        public static string ValueIsDateStart = "لطفا مقدار ";
        public static string ValueIsDateEnd = " را به صورت تاریخ، مانند(1392/01/01) وارد نمایید.";
        public static string YearIsValid  = "لطفا مقدار سال را در محدوده 1300 تا 1500 وارد نمایید ";
        public static string MonthIsValid = "لطفا مقدار ماه را در محدوده 01 تا 12 وارد نمایید ";
        public static string DayIsValid = "لطفا مقدار روز را برای 6 ماه اول سال در محدوده 01 تا 31 و برای 6 ماه دوم سال در محدوده 01 تا 30 وارد نمایید ";        
        public static string DateIsNotValid = "مقادیر وارد شده به ازای تاریخ ها معتبر نمی باشد";
        public static string TwoDayIsNotValid = "روز های دو تاریخ (از، تا) بایستی با یکدیگر برابر باشد.";
        public static string TwoYearIsNotValid = "سالهای دو تاریخ (از، تا) بایستی با یکدیگر برابر باشد.";
        public static string TwoMonthIsNotValid = "مقدار ماه از تاریخ بایستی از مقدار ماه تا تاریخ کوچکتر باشد.";
        public static string CurrentYearNotValid = "سالهای دو تاریخ (از،تا) بایستی با تاریخ جاری برابر باشد.";
        public static string ScopeOfDateNotValid = "محدوده تاریخ وارد شده معتبر نمی باشد.";
    }

}
