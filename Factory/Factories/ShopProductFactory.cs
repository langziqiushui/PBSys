using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JK.YunMall.Domain.Mall;
using JK.YunMall.Domain.Credits;
using JK.YunMall.Domain;
using JK.YunMall.Factory.IM;
using JK.YunMall.Domain.IM;
using JK.YunMall.Core;

namespace JK.YunMall.Factory
{
    /// <summary>
    /// 店铺商品工厂
    /// </summary>
    public class ShopProductFactory
    {
        #region 变量

        /// <summary>
        /// 购物车服务对象。
        /// </summary>
        private static IServices.Mall.IShopcartServices serviceShopCart = Factory.ServicesFactory.CreateMallShopcartServices();

        #endregion

        #region M站产品相关

        /// <summary>
        /// 获取M站首页商品信息
        /// </summary>
        /// <param name="ShopName"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static List<PageProductInfo> GetMWebIndexProductInfo(string shopName, ref int totalRecord)
        {
            return GetMWebProductList(shopName, 10, 1, 2, true, null, ref totalRecord);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopName"></param>
        /// <param name="pageSize"></param>
        /// <param name="currIndex"></param>
        /// <param name="sort"></param>
        /// <param name="isPublic">店铺是否发布</param>
        /// <param name="keyword">关键字</param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static List<PageProductInfo> GetMWebProductList(string shopName, int pageSize, int currIndex,
            int? sort, bool? isPublic, string keyword, ref int totalRecord)
        {
            var list = new List<PageProductInfo>();
            //获取店铺产品
            totalRecord = 0;
            var data = Factory.ServicesFactory.CreateMallProductServices().GetData_Paging_ForShop(
                pageSize, currIndex, ref totalRecord, null, null, null, shopName, isPublic, null, sort, keyword, false);
            var productInfoList = (List<ProductInfo>)data[ProductInfo.TableName]; //产品列表
            var propertyList = (List<ProductpropertyInfo>)data[ProductpropertyInfo.TableName]; //产品列表
            foreach (var p in productInfoList)
            {
                list.Add(new PageProductInfo()
                {
                    Product = p,
                    ProductProperty = propertyList.FirstOrDefault(n => n.ProductID == p.ProductID)
                });
            }
            return list;
        }
        /// <summary>
        /// M站 通过场馆编号 获取所有产品列表
        /// </summary>
        /// <param name="museumid">场馆ID</param>
        /// <param name="keyword"></param>
        /// <param name="sorttype"></param>
        /// <param name="category"></param>
        /// <param name="brand"></param>
        /// <param name="th">是否为特惠商品</param>
        /// <param name="pagesize"></param>
        /// <param name="currpage"></param>
        /// <param name="totalrecord"></param>
        /// <returns></returns>
        public static List<PageProductInfo> GetMallProductList_ForMuseumid(string museumid, string keyword, int? sorttype, int? category, int? brand, bool? th, int pagesize, int currpage, ref int totalrecord)
        {
            var list = new List<PageProductInfo>();
            //获取店铺产品
            totalrecord = 0;
            var datas = Factory.ServicesFactory.CreateMallProductServices().GetData_Paging(
                   pagesize, currpage, ref totalrecord, keyword, category, null, null, true, null,
                   null, th, true, null, null, null, brand, museumid, sorttype
           );
            var productInfoList = (List<ProductInfo>)datas[ProductInfo.TableName]; //产品列表
            var propertyList = (List<ProductpropertyInfo>)datas[ProductpropertyInfo.TableName]; //产品列表

            foreach (var p in productInfoList)
            {
                list.Add(new PageProductInfo()
                {
                    Product = p,
                    ProductProperty = propertyList.FirstOrDefault(n => n.ProductID == p.ProductID)
                });
            }
            return list;
        }
        #endregion

        #region App所有产品列表

        /// <summary>
        /// App获取所有产品列表
        /// </summary>
        /// <param name="shopuser"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="currpage"></param>
        /// <param name="totalrecord"></param>
        /// <returns></returns>
        public static List<PageProductInfo> GetAppAllProductList(string shopuser, string key, int pagesize, int currpage, ref int totalrecord)
        {
            var list = new List<PageProductInfo>();
            //获取店铺产品
            totalrecord = 0;
            var datas = Factory.ServicesFactory.CreateMallProductServices().GetData_Paging(
                   pagesize, currpage, ref totalrecord, key, null, null, null, true, null,
                   null, null, true, null, null, shopuser, null, null, null
           );
            var productInfoList = (List<ProductInfo>)datas[ProductInfo.TableName]; //产品列表
            var propertyList = (List<ProductpropertyInfo>)datas[ProductpropertyInfo.TableName]; //产品列表

            foreach (var p in productInfoList)
            {
                list.Add(new PageProductInfo()
                {
                    Product = p,
                    ProductProperty = propertyList.FirstOrDefault(n => n.ProductID == p.ProductID)
                });
            }
            return list;
        }


        /// <summary>
        /// App获取所有分类产品列表
        /// </summary>
        /// <param name="shopuser"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="currpage"></param>
        /// <param name="totalrecord"></param>
        /// <param name="ProCateID"></param>
        /// <returns></returns>
        public static List<PageProductInfo> GetAppAllProductList(string shopuser, string key, int pagesize, int currpage, int? ProCateID, bool? isHot, ref int totalrecord, int sort)
        {
            var list = new List<PageProductInfo>();
            //获取店铺产品

            totalrecord = 0;
            if (ProCateID == 0)
            {
                ProCateID = null;
            }

            var datas = Factory.ServicesFactory.CreateMallProductServices().GetData_Paging(
                   pagesize, currpage, ref totalrecord,
                   key, ProCateID, null, null, true, isHot, null, null, true, null, null, shopuser, null, null, sort
               );
            var productInfoList = (List<ProductInfo>)datas[ProductInfo.TableName]; //产品列表
            var propertyList = (List<ProductpropertyInfo>)datas[ProductpropertyInfo.TableName]; //产品列表

            foreach (var p in productInfoList)
            {
                list.Add(new PageProductInfo()
                {
                    Product = p,
                    ProductProperty = propertyList.FirstOrDefault(n => n.ProductID == p.ProductID)
                });
            }
            return list;
        }

        #endregion

        #region 购物车相关

        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResponseAddToCart AddToCart(RequestAddToCart request)
        {
            try
            {
                //查找产品属性
                var propertyEntity = Factory.ServicesFactory.CreateMallProductpropertyServices().GetData(request.PropertyID);
                if (null == propertyEntity)
                {
                    return new ResponseAddToCart()
                    {
                        IsSuccess = false,
                        Message = "没有找到属性信息"
                    };
                }

                if (propertyEntity.ProductID != request.ProductID)
                {
                    return new ResponseAddToCart()
                    {
                        IsSuccess = false,
                        Message = "产品不符合"
                    };
                }

                //判断现有购物车内是否有相同产品
                var ilist = Factory.ServicesFactory.CreateMallShopcartServices().GetData_ByUserName(
                        request.UserName).Where(c => c.ProductType == (byte)ShopCartProductTypes.云商城商品).ToList(); ;
                var hasSameProduct = ilist.FirstOrDefault(n => n.ProductID == request.ProductID &&
                    n.ProductPropertyID == request.PropertyID);

                var cartotalProductcount = 0; //购物车内产品总数

                
                foreach (var item in ilist)
                    cartotalProductcount += item.NumBuy;

                if (hasSameProduct != null)
                {
                    #region 购物车内有产品

                    var pro_totalcount = hasSameProduct.NumBuy + request.BuyCount;
                    if (pro_totalcount > propertyEntity.NumStock)
                    {
                        return new ResponseAddToCart() { IsSuccess = false, Message = "补货中" };
                    }
                    //更新购物车
                    hasSameProduct.NumBuy = pro_totalcount;
                    hasSameProduct.SalePrice = propertyEntity.MarketPrice; //重新赋值价格
                    hasSameProduct.Score = propertyEntity.MarketPrice - propertyEntity.SalePrice;
                    var obj = Factory.ServicesFactory.CreateMallShopcartServices().Modify(hasSameProduct);

                    if (Convert.ToInt32(obj) > 0)
                    {
                        cartotalProductcount += request.BuyCount;
                        return new ResponseAddToCart() { IsSuccess = true, ShopCartID = hasSameProduct.ShopCartID, TotalCount = cartotalProductcount };
                    }
                    else
                        return new ResponseAddToCart() { IsSuccess = false, Message = "添加购物车失败" };

                    #endregion
                }
                else
                {
                    #region 购物车内无产品

                    if (propertyEntity.NumStock < request.BuyCount)
                    {
                        return new ResponseAddToCart() { IsSuccess = false, Message = "补货中" };
                    }
                    //获取产品信息
                    var productEntity = Factory.ServicesFactory.CreateMallProductServices().GetData(request.ProductID);
                    //正式加入到购物车
                    var shopCartEntity = new ShopcartInfo()
                    {
                        CreateTime = DateTime.Now,
                        NumBuy = request.BuyCount,
                        ProductID = request.ProductID,
                        MainImage = propertyEntity.MainImage,
                        UserName = request.UserName,
                        SupplerUserName = productEntity.UserName,
                        SalePrice = propertyEntity.MarketPrice,
                        ProductPropertyID = propertyEntity.ProductPropertyID,
                        ProductName = productEntity.Name,
                        PictureFolderName = productEntity.PictureFolderName,
                        PropertyName = propertyEntity.PropertyName,
                        Score = propertyEntity.MarketPrice - propertyEntity.SalePrice
                    };
                    int shopcartid = 0;
                    var obj = Factory.ServicesFactory.CreateMallShopcartServices().Add(shopCartEntity, ref shopcartid);
                    if (shopcartid > 0)
                    {
                        cartotalProductcount += request.BuyCount; //购物车内总数
                        return new ResponseAddToCart() { IsSuccess = true, ShopCartID = shopcartid, TotalCount = cartotalProductcount };
                    }
                    else
                    {
                        return new ResponseAddToCart()
                        {
                            IsSuccess = false,
                            Message = "添加到购物车失败"
                        };
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                return new ResponseAddToCart()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }

        /// <summary>
        /// 懒猫产品添加到购物车
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResponseUpateShopCart UpdateToCart(RequestLCatUpdateToCart request)
        {
            try
            {
                var isSuccess = false;
                //获取用户加入购物车的产品信息
                var entityUserProduct = Factory.ServicesFactory.CreateLCatUserproductServices().GetData(request.UserProductID);
                //判断现有购物车内是否有相同产品
                var dataShopCart = Factory.ServicesFactory.CreateMallShopcartServices().GetData_ByUserName(request.UserName, (byte)ShopCartProductTypes.懒猫社长商品, request.Shopuser);
                var entityShopCart = dataShopCart.FirstOrDefault(n => n.ProductID == request.UserProductID);

                //购物车内产品总数 
                var numTotalShopCart = 0;
                foreach (var item in dataShopCart)
                    numTotalShopCart += item.NumBuy;

                if (null != entityShopCart)
                {
                    #region 购物车内有产品

                    var numTotalBuy = 0;
                    if (request.Type == 1)
                    {
                        //添加商品。
                        numTotalBuy = entityShopCart.NumBuy + request.BuyCount;
                        numTotalShopCart += request.BuyCount;

                        //如果购买数量超过库存量。
                        if (numTotalBuy > entityUserProduct.NumTotalStock)
                            return new ResponseUpateShopCart() { IsSuccess = false, Message = "补货中" };
                    }
                    else if (request.Type == 0)
                    {
                        //删除商品。
                        numTotalBuy = entityShopCart.NumBuy - request.BuyCount;
                        numTotalShopCart -= request.BuyCount;
                    }
                    
                    //如果将要购买数量为0则从购买车删除商品。
                    if (numTotalBuy <= 0)
                    {
                        var productDelected = dataShopCart.SingleOrDefault(c => c.ProductID == request.UserProductID && c.SupplerUserName == request.Shopuser && c.UserName == request.UserName && c.ProductType == (byte)ShopCartProductTypes.懒猫社长商品);

                        if (productDelected != null)
                            isSuccess = Factory.ServicesFactory.CreateMallShopcartServices().Delete(productDelected.ShopCartID);
                    }
                    else
                    {
                        //更新购物车
                        entityShopCart.NumBuy = numTotalBuy;
                        entityShopCart.SalePrice = entityUserProduct.Price; //重新赋值价格

                        var obj = Factory.ServicesFactory.CreateMallShopcartServices().Modify(entityShopCart);

                        if (Convert.ToInt32(obj) > 0)
                            isSuccess = true;
                    }

                    if (isSuccess)
                        return new ResponseUpateShopCart() { IsSuccess = true, TotalCount = numTotalShopCart };
                    else
                        return new ResponseUpateShopCart() { IsSuccess = false, Message = "更新购物车失败" };

                    #endregion
                }
                else
                {
                    #region 购物车内无产品
                    if (request.Type == 0)
                    {
                        return new ResponseUpateShopCart() { IsSuccess = false, Message = "购物车内没有此产品" };
                    }
                    if (entityUserProduct.NumTotalStock < request.BuyCount)
                    {
                        return new ResponseUpateShopCart() { IsSuccess = false, Message = "补货中" };
                    }
                    var productEntity = Factory.ServicesFactory.CreateLCatProductServices().GetData(entityUserProduct.ProductID);
                    //正式加入到购物车
                    var shopCartEntity = new ShopcartInfo()
                    {
                        CreateTime = DateTime.Now,
                        NumBuy = request.BuyCount,
                        ProductID = request.UserProductID,
                        MainImage = productEntity.MainImage,
                        UserName = request.UserName,
                        SupplerUserName = request.Shopuser,
                        SalePrice = productEntity.Price,
                        ProductPropertyID = 0,
                        ProductName = productEntity.Name,
                        PictureFolderName = productEntity.PictureFolderName,
                        PropertyName = string.Empty,
                        Score = 0,
                        ProductType = (byte)ShopCartProductTypes.懒猫社长商品

                    };
                    int shopcartid = 0;
                    var obj = Factory.ServicesFactory.CreateMallShopcartServices().Add(shopCartEntity, ref shopcartid);
                    if (shopcartid > 0)
                    {
                        numTotalShopCart += request.BuyCount; //购物车内总数
                        return new ResponseUpateShopCart() { IsSuccess = true, TotalCount = numTotalShopCart };
                    }
                    else
                    {
                        return new ResponseUpateShopCart()
                        {
                            IsSuccess = false,
                            Message = "添加到购物车失败"
                        };
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                return new ResponseUpateShopCart()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }

        /// <summary>
        /// 微品产品更新购物车
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResponseUpateShopCart UpdateToWPCart(RequestLCatUpdateToCart request)
        {
            try
            {
                var isSuccess = false;
                //获取用户加入购物车的产品信息
                var userProductEntity = Factory.ServicesFactory.CreateMallShopproductServices().GetData(request.UserName, request.UserProductID);
                //判断现有购物车内是否有相同产品getLCatShopCartList(request.UserName);
                var ilist = Factory.ServicesFactory.CreateMallShopcartServices().GetData_ByUserName(request.UserName).Where(c => c.ProductType == (byte)ShopCartProductTypes.云商城商品 && c.SupplerUserName == request.Shopuser).ToList();
                var hasSameProduct = ilist.FirstOrDefault(n => n.ProductID == request.UserProductID);

                var cartotalProductcount = 0; //购物车内产品总数

                foreach (var item in ilist)
                    cartotalProductcount += item.NumBuy;

                if (hasSameProduct != null)
                {
                    #region 购物车内有产品

                    //计算更新到购物车中的产品数量
                    var pro_totalcount = 0;
                    if (request.Type == 1)
                    {
                        pro_totalcount = hasSameProduct.NumBuy + request.BuyCount;
                        cartotalProductcount += request.BuyCount;
                    }
                    else if (request.Type == 0)
                    {
                        pro_totalcount = hasSameProduct.NumBuy - request.BuyCount;
                        cartotalProductcount -= request.BuyCount;
                    }

                    if (pro_totalcount <= 0)
                    {
                        var productDelected = ilist.SingleOrDefault(c => c.ProductID == request.UserProductID && c.SupplerUserName == request.Shopuser && c.UserName == request.UserName && c.ProductType == (byte)ShopCartProductTypes.云商城商品);

                        if (productDelected != null)
                            isSuccess = Factory.ServicesFactory.CreateMallShopcartServices().Delete(productDelected.ShopCartID);
                    }
                    else
                    {
                        //更新购物车
                        hasSameProduct.NumBuy = pro_totalcount;
                        //hasSameProduct.SalePrice = userProductEntity.Price; //重新赋值价格

                        var obj = Factory.ServicesFactory.CreateMallShopcartServices().Modify(hasSameProduct);

                        if (Convert.ToInt32(obj) > 0)
                            isSuccess = true;
                    }

                    if (isSuccess)
                        return new ResponseUpateShopCart() { IsSuccess = true, TotalCount = cartotalProductcount };
                    else
                        return new ResponseUpateShopCart() { IsSuccess = false, Message = "更新购物车失败" };

                    #endregion
                }
                else
                {
                    #region 购物车内无产品
                    if (request.Type == 0)
                    {
                        return new ResponseUpateShopCart() { IsSuccess = false, Message = "购物车内没有此产品" };
                    }
                    var productEntity = Factory.ServicesFactory.CreateMallProductServices().GetData(userProductEntity.ProductID);
                    //正式加入到购物车
                    var shopCartEntity = new ShopcartInfo()
                    {
                        CreateTime = DateTime.Now,
                        NumBuy = request.BuyCount,
                        ProductID = request.UserProductID,
                        MainImage = productEntity.MainImage,
                        UserName = request.UserName,
                        SupplerUserName = request.Shopuser,
                        //SalePrice =  productEntity.Price,
                        ProductPropertyID = 0,
                        ProductName = productEntity.Name,
                        PictureFolderName = productEntity.PictureFolderName,
                        PropertyName = string.Empty,
                        Score = 0,
                        ProductType = (byte)ShopCartProductTypes.云商城商品

                    };
                    int shopcartid = 0;
                    var obj = Factory.ServicesFactory.CreateMallShopcartServices().Add(shopCartEntity, ref shopcartid);
                    if (shopcartid > 0)
                    {
                        cartotalProductcount += request.BuyCount; //购物车内总数
                        return new ResponseUpateShopCart() { IsSuccess = true, TotalCount = cartotalProductcount };
                    }
                    else
                    {
                        return new ResponseUpateShopCart()
                        {
                            IsSuccess = false,
                            Message = "添加到购物车失败"
                        };
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                return new ResponseUpateShopCart()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }






        /// <summary>
        /// 查询指定用户 购物车内产品数量
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int QueryUserCartNum(string userName)
        {
            var shopcartList = Factory.ServicesFactory.CreateMallShopcartServices().GetData_ByUserName(userName);
            if (shopcartList != null)
            {
                var total = 0;
                foreach (var item in shopcartList)
                    total += item.NumBuy;
                return total;
            }
            else
                return 0;
        }

        /// <summary>
        /// 查询指定用户 购物车内内容
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IList<ShopcartInfo> GetUserShopCarts(string userName)
        {
            return Factory.ServicesFactory.CreateMallShopcartServices().GetData_ByUserName(userName);
        }


        /// <summary>
        /// 查询指定 购物ID 内内容
        /// </summary>
        /// <param name="shopCartID "></param>
        /// <returns></returns>
        public static ShopcartInfo GetIDShopCarts(int shopCartID)
        {
            return serviceShopCart.GetData(shopCartID);
        }

        /// <summary>
        /// 从购物车内删除指定产品。
        /// </summary>
        /// <param name="shopcartID">购物车商品编号</param>
        /// <param name="shopUser">购物者用户名</param>
        /// <param name="checkedShopCartIDs">需要购买的购物车商品编号集(以|号分隔)</param>
        /// <returns></returns>
        public static ResponseModifyCard DelProFromCart(int shopcartID, string shopUser, string checkedShopCartIDs)
        {
            var entityShopCart = serviceShopCart.GetData(shopcartID);
            if (null == entityShopCart || !entityShopCart.UserName.IgnoreCaseEquals(shopUser))
            {
                return new ResponseModifyCard()
                {
                    IsSuccess = false,
                    Message = "购物车中不存在此商品！"
                };
            }

            var servers = Factory.ServicesFactory.CreateMallShopcartServices();
            var obj = servers.Delete(shopcartID);
            if (Convert.ToInt16(obj) <= 0)
            {
                return new ResponseModifyCard()
                {
                    IsSuccess = false,
                    Message = "删除购物车商品失败！",
                    MarketPrice = 0,
                    TotalPostFee = 0,
                    TotalJiFen = 0,
                };
            }

            return CalculateShopCart(shopUser, checkedShopCartIDs);
        }



        /// <summary>
        /// 修改购物车内产品数量
        /// </summary>
        /// <param name="shopcartID">购物车商品编号</param>
        /// <param name="newNum">新数量</param>
        /// <param name="shopUser">购物者用户名</param>
        /// <param name="checkedShopCartIDs">需要购买的购物车商品编号集(以|号分隔)</param>
        /// <returns></returns>
        public static ResponseModifyCard ModifyCartNum(int shopcartID, int newNum, string shopUser, string checkedShopCartIDs)
        {
            var entityShopCart = serviceShopCart.GetData(shopcartID);
            if (null == entityShopCart || !entityShopCart.UserName.IgnoreCaseEquals(shopUser))
            {
                return new ResponseModifyCard()
                {
                    IsSuccess = false,
                    Message = "购物车中不存在此商品！"
                };
            }

            //查看产品信息
            var entityProductProperty = Factory.ServicesFactory.CreateMallProductpropertyServices().GetData(entityShopCart.ProductPropertyID);
            if (newNum > entityProductProperty.NumStock)
            {
                return new ResponseModifyCard()
                {
                    IsSuccess = false,
                    Message = "补货中！"
                };
            }

            //修改购买数量
            entityShopCart.NumBuy = newNum;
            var obj = Factory.ServicesFactory.CreateMallShopcartServices().Modify(entityShopCart);
            if (Convert.ToInt16(obj) <= 0)
            {
                return new ResponseModifyCard()
                {
                    IsSuccess = false,
                    Message = "修改购买数量失败！"
                };
            }

            return CalculateShopCart(shopUser, checkedShopCartIDs);
        }

        #endregion

        #region 计算购买车中商品价格

        /// <summary>
        /// 计算购买车中商品价格。
        /// </summary>
        /// <param name="shopUser">购物者用户名</param>
        /// <param name="checkedShopCartIDs">需要购买的购物车商品编号集(以|号分隔，null或空字符串表示全部商品)</param>
        /// <returns></returns>
        public static ResponseModifyCard CalculateShopCart(string shopUser, string checkedShopCartIDs)
        {
            //获取购物车商品。
            var dataShopCart = serviceShopCart.GetData_ByUserName(shopUser);
            return CalculateShopCart(dataShopCart, checkedShopCartIDs);
        }

        /// <summary>
        /// 计算购买车中商品价格。
        /// </summary>
        /// <param name="shopUser">购物者用户名</param>
        /// <param name="checkedShopCartIDs">需要购买的购物车商品编号集(以|号分隔，null表示全部商品)</param>
        /// <returns></returns>
        public static ResponseModifyCard CalculateShopCart(IList<ShopcartInfo> dataShopCart, string checkedShopCartIDs)
        {
            var response = new ResponseModifyCard();
            var arrShopCartIDs = null != checkedShopCartIDs ? checkedShopCartIDs.Split("|").ToIntArray() : null;

            //总市场价格
            var toatlaMarketPrice = 0m;
            //总积分
            var totalJifen = 0m;
            //总邮费
            var totalPostFee = 0m;
            //商品种数。
            var numProduct = 0;
            //总佣金
            var totalYongjin = 0m;

            var requestDataProductBuy = new List<JK.YunMall.Domain.Mall.RequestCalculateProductCarriage.ProductBuyInfo>();
            foreach (var _entityShopCart in dataShopCart)
            {
                //如果商品已下线或库存不够。
                if (!_entityShopCart.IsSaleing || _entityShopCart.NumStock <= 0)
                    continue;
                //如果不包含在选择列表中。
                if (null != arrShopCartIDs && !arrShopCartIDs.Contains(_entityShopCart.ShopCartID))
                    continue;

                numProduct += 1;
                toatlaMarketPrice += _entityShopCart.SalePrice * _entityShopCart.NumBuy;
                totalJifen += Factory.MallUtil.CalculateProductScore(_entityShopCart.Score) * _entityShopCart.NumBuy;
                totalYongjin += JK.YunMall.Factory.MallUtil.CalculateOrderUser(
                    JK.YunMall.Factory.MUserManager.LoginUser,
                    MallUtil.GetReturnCommissionType(_entityShopCart.ReturnCommissionType),
                    _entityShopCart.ProductPropertyID
                ) * _entityShopCart.NumBuy;
                requestDataProductBuy.Add(new JK.YunMall.Domain.Mall.RequestCalculateProductCarriage.ProductBuyInfo()
                {
                    NumBuy = _entityShopCart.NumBuy,
                    ProductID = _entityShopCart.ProductID,
                    SalePrice = _entityShopCart.SalePrice
                });
            }

            //获取邮费。
            var responseCalculateProductCarriage = MallUtil.CalculateProductCarriage(new RequestCalculateProductCarriage()
            {
                DataProductBuy = requestDataProductBuy
            });
            foreach (var item in responseCalculateProductCarriage.DataProductCarriage)
                totalPostFee += item.TotalCarriageFee;

            response.IsSuccess = true;
            response.MarketPrice = toatlaMarketPrice;
            response.TotalJiFen = totalJifen;
            response.TotalPostFee = totalPostFee;
            response.ShopCartItem = numProduct;
            response.TotalYongJin = totalYongjin;

            return response;
        }

        #endregion

        #region  懒猫的购买车

        /// <summary>
        /// 通过购买者的用户名和懒猫社长的用户名 获取懒猫购物车。
        /// </summary>
        /// <param name="userName">购买者的用户名</param>
        /// <param name="supplerUsername">懒猫社长的用户名</param>
        /// <returns></returns>
        public static IList<ShopcartInfo> getLCatShopCartList(string userName, string supplerUsername)
        {
            var response = new List<ShopcartInfo>();

            //获取购物车商品。
            var dataShopCart = serviceShopCart.GetData_LCart(userName, supplerUsername);

            string productids = string.Empty;
            foreach (var item in dataShopCart)
            {
                productids += item.ProductID.ToString() + "|";
            }

            var dataList = ServicesFactory.CreateLCatUserproductServices().GetData_UserProductIDs(productids);
            foreach (var _entityShopCart in dataShopCart)
            {
                var data = dataList.Where(g => (g.UserProductID == _entityShopCart.ProductID)).ToList().FirstOrDefault();

                //实体不存在，自动跳过此产品。
                if (data == null)
                    continue;

                //未通过审核
                if (data.IsVerify.HasValue)
                    if (data.IsVerify.Value == false)
                        continue;

                //未发布、补货中
                if (!data.IsPublic || data.NumTotalStock <= 0)
                    continue;

                //增加一层过滤，只显示当前所在浏览的店铺的购物车信息，别的商家的购物的产品信息即使有也不显示。
                //ProductUser  nvarchar(25)    商品所有者用户名 
                if (data.ProductUser != supplerUsername)
                    continue;

                //添加 有效的产品信息到购物车
                ShopcartInfo cartInfo = new ShopcartInfo();

                cartInfo.ShopCartID = _entityShopCart.ShopCartID;
                cartInfo.UserName = _entityShopCart.UserName;
                cartInfo.SupplerUserName = _entityShopCart.SupplerUserName;
                cartInfo.ProductType = _entityShopCart.ProductType;
                cartInfo.ProductID = _entityShopCart.ProductID; //商品编号   
                cartInfo.ProductName = data.ProductName; //商品名称  
                cartInfo.ProductPropertyID = _entityShopCart.ProductPropertyID;
                cartInfo.PropertyName = _entityShopCart.PropertyName;
                cartInfo.MainImage = _entityShopCart.MainImage;
                cartInfo.PictureFolderName = _entityShopCart.PictureFolderName;
                cartInfo.NumBuy = _entityShopCart.NumBuy;//   购物数量  
                cartInfo.SalePrice = data.Price; //  销售价  
                cartInfo.Score = _entityShopCart.Score;
                cartInfo.CreateTime = _entityShopCart.CreateTime;

                //添加可以购买的产品到购物车中。
                response.Add(cartInfo);

            }


            return response;
        }

        #endregion

        #region 和下单相关

        /// <summary>
        /// 获取下单界面所需要的信息
        /// </summary>
        /// <param name="shopcartIDs">购物车ids</param>
        /// <param name="userName">当前登录用户名</param>
        /// <param name="cityID">收货地址对应的城市</param>
        /// <returns></returns>
        public static List<BookOrderPageInfo> GetBookOrderInfo(int[] shopcartIDs, string userName, int? cityID)
        {
            //获取当前用户所有购物车信息
            var dataShopCart = Factory.ServicesFactory.CreateMallShopcartServices().GetData_ByUserName(userName).Where(g => { return shopcartIDs.Contains(g.ShopCartID); }).ToList();

            //判断商品是否在发货区域。
            //如果当前用户没有填写收货地址则默认为“已在配送区域”。
            foreach (var _entity in dataShopCart)
                _entity.IsInCourierArea = null != cityID ? MallUtil.IsInCourierArea(cityID.Value, _entity.ProductID) : true;

            //分组所有供应商。
            var dicSupplier = new Dictionary<string, List<ShopcartInfo>>();
            foreach (var _entityShopCart in dataShopCart)
            {
                if (!dicSupplier.ContainsKey(_entityShopCart.SupplerUserName))
                    dicSupplier.Add(_entityShopCart.SupplerUserName, new List<ShopcartInfo>() { _entityShopCart });
                else
                    dicSupplier[_entityShopCart.SupplerUserName].Add(_entityShopCart);
            }

            //分组统计运费。
            var dataBookOrderPage = new List<BookOrderPageInfo>();
            foreach (var item in dicSupplier)
            {
                //计算当前分组的运费。
                var totalPostFee = 0m;
                var requestList = new List<JK.YunMall.Domain.Mall.RequestCalculateProductCarriage.ProductBuyInfo>();
                foreach (var p in item.Value)
                {
                    //如果商品在发货区域。
                    if (p.IsInCourierArea)
                    {
                        requestList.Add(new JK.YunMall.Domain.Mall.RequestCalculateProductCarriage.ProductBuyInfo()
                        {
                            NumBuy = p.NumBuy,
                            ProductID = p.ProductID,
                            SalePrice = p.SalePrice
                        });
                    }
                }
                var response = MallUtil.CalculateProductCarriage(new RequestCalculateProductCarriage()
                {
                    DataProductBuy = requestList,
                    CityID = cityID
                });

                //统计总运费。
                foreach (var p in response.DataProductCarriage)
                    totalPostFee += p.TotalCarriageFee;

                dataBookOrderPage.Add(new BookOrderPageInfo()
                {
                    SupplerName = item.Key,
                    Carts = item.Value,
                    TotalPost = totalPostFee
                });
            }

            return dataBookOrderPage;
        }


        /// <summary>
        /// 商城下订单
        /// </summary>
        /// <param name="shopcarts"></param>
        /// <param name="addressInfo"></param>
        /// <returns></returns>
        public static ResponseSubmitOrder SubmitOrder(string shopcartIDs, int addressId, string userName, string remark)
        {
            var shopcartIDArray = shopcartIDs.Split(',');
            var idsList = new List<int>();
            foreach (var item in shopcartIDArray)
            {
                if (!string.IsNullOrEmpty(item))
                    idsList.Add(int.Parse(item));
            }
            //获取当前用户所有购物车信息
            var shopcarts = Factory.ServicesFactory.CreateMallShopcartServices().GetData_ByUserName(userName);

            //排除本次没选中的内容
            var hasSelshopCarts = shopcarts.Where(g => { return idsList.ToArray().Contains(g.ShopCartID); });

            //收货地址信息  姓名|手机|省|市|地址|邮编
            var entityAddress = Factory.ServicesFactory.CreateMemberDeliveraddressServices().GetData(addressId);

            var request = new RequestPostOrdeer();

            if (null != entityAddress)
            {
                request.Address = entityAddress.ProvinceName + " " + entityAddress.CityName + " " + entityAddress.Address;
                request.Linkman = entityAddress.RealName;
                request.Mobile = entityAddress.Mobile;
                request.ZIP = entityAddress.ZIP;
                request.OrderUser = userName;
                request.Remark = remark;
            }

            #region 获取总运费
            var totalPostFee = 0m;
            var totalAmount = 0m; //总价格

            List<RequestPostOrdeer.ProductBuy> orderProducts = new List<RequestPostOrdeer.ProductBuy>();

            //构建运费需要的内容
            var requestList = new List<JK.YunMall.Domain.Mall.RequestCalculateProductCarriage.ProductBuyInfo>();
            foreach (var p in hasSelshopCarts)
            {
                var dataproductbuy = new JK.YunMall.Domain.Mall.RequestCalculateProductCarriage.ProductBuyInfo()
                {
                    NumBuy = p.NumBuy,
                    ProductID = p.ProductID,
                    SalePrice = p.SalePrice,
                };
                requestList.Add(dataproductbuy);       //添加请求参数
                totalAmount += p.SalePrice * p.NumBuy; //计算订单金额
                orderProducts.Add(new RequestPostOrdeer.ProductBuy()
                {
                    NumBuy = p.NumBuy,
                    Price = p.SalePrice,
                    ProductID = p.ProductID,
                    ProductProperty = p.ProductPropertyID,
                    ShopCartID = p.ShopCartID
                });
            }

            var responseYunFei = MallUtil.CalculateProductCarriage(new RequestCalculateProductCarriage()
            {
                CityID = entityAddress.CityID,  //收货城市编号
                DataProductBuy = requestList
            });

            if (responseYunFei.DataProductCarriage.Count > 0)
            {
                foreach (var p in responseYunFei.DataProductCarriage)
                {
                    totalPostFee += p.TotalCarriageFee;
                }
            }
            #endregion

            request.TotalCarriage = totalPostFee; //总运费
            request.TotalMoney = totalAmount + totalPostFee;
            request.DataProductBuy = orderProducts;//产品
            request.DeliverCityID = entityAddress.CityID.Value;  //收货城市编号

            var response = Factory.ServicesFactory.CreateMallOrderServices().PostOrder(request);
            var orderNos = new List<string>();
            if (response.IsSuccess)
                orderNos.Add(response.OrderNo);

            return new ResponseSubmitOrder()
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                OrderNos = orderNos
            };
        }

        /// <summary>
        /// 查询 积分可用情况  //如果都勾选,使用顺序 余额/消费积分/分享积分
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResponseUseJiFen QueryUseJiFen(RequestUseJiFen request)
        {
            var response = new ResponseUseJiFen()
            {
                IsSuccess = false,
                Message = "暂未实现"
            };

            //查询用户积分情况
            var entityScore = Factory.ServicesFactory.CreateCreditsScoreServices().GetData(request.UserName);
            if (null == entityScore)
                return response;

            //总抵扣。
            var totalDiKou = 0m;

            #region 购物积分

            var gouWuPay = 0m;
            response.TotalGouWu = entityScore.ShopScore;
            if (request.IsUseGWJF)
            {
                //根据传入的订单购物积分抵扣上限获取可抵积分。
                var shopScore = Math.Min(request.ProductGouWu, entityScore.ShopScore);
                response.IsUseGWJF = true;
                response.UseGouWu = Math.Min(shopScore, request.TotalAmount / DBConfigFactory.Member_GouWuJiFen);
                gouWuPay = response.UseGouWu;
                totalDiKou += gouWuPay * DBConfigFactory.Member_GouWuJiFen;
            }

            #endregion

            #region 分享积分

            var fenXiangPay = 0m;
            response.TotalFenXiang = entityScore.InviteScore;
            if (request.IsUseFXJF)
            {
                response.IsUseFXJF = true;
                response.UseFenXiang = Math.Min(entityScore.InviteScore, request.TotalAmount / DBConfigFactory.Member_FenXiangJiFen);
                response.UseFenXiang = Math.Min(response.UseFenXiang, DBConfigFactory.Mall_ReturnCommissionConfig.每单使用分享积分抵扣最大值);
                fenXiangPay = Math.Min(entityScore.InviteScore, (request.TotalAmount - totalDiKou) / DBConfigFactory.Member_FenXiangJiFen);
                fenXiangPay = Math.Min(fenXiangPay, DBConfigFactory.Mall_ReturnCommissionConfig.每单使用分享积分抵扣最大值);
                totalDiKou += fenXiangPay * DBConfigFactory.Member_FenXiangJiFen;
            }

            #endregion

            #region 用户余额

            var yuePay = 0m;
            response.TotalYUE = entityScore.CashBalance + entityScore.NotCashBalance;
            if (request.IsUseYuE)
            {
                response.IsUseYuE = true;
                var totalBalance = entityScore.CashBalance + entityScore.NotCashBalance;
                response.UseYUE = Math.Min(totalBalance, request.TotalAmount);
                yuePay = Math.Min(totalBalance, request.TotalAmount - totalDiKou);
                totalDiKou += yuePay;
            }

            #endregion

            response.IsSuccess = true;
            response.Message = "成功";
            response.TotalAmount = request.TotalAmount;
            response.RealPayAmount = Math.Max(request.TotalAmount - totalDiKou, 0);

            //按以下格式加密字符串。
            //实付金额|余额支付|消费积分|分享积分|购物积分|总金额
            var cryptManager = new Core.CryptographyManager(Core.EncryptionFormats.DES2QueryString);
            response.DataStr = cryptManager.Encrypt(
                string.Format(
                "{0}|{1}|{2}|{3}|{4}|{5}",
                response.RealPayAmount, yuePay, response.UseXiaoFei, fenXiangPay, gouWuPay,
                response.TotalAmount
                )
            );

            return response;
        }

        #endregion

        #region 使用积分支付

        /// <summary>
        /// 使用积分支付订单。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResponseJiFenPay PayByJiFen(RequestJiFenPay request)
        {
            try
            {
                return Factory.ServicesFactory.CreateCreditsPayorderServices().OnJiFenPayPaySuccess(request);
            }
            catch (Exception ex)
            {
                Factory.LogFactory.Write(ApplicationTypes.Pay, ex.ToString());
                return new ResponseJiFenPay()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        #endregion

        #region 品牌推荐
        /// <summary>
        /// 获取推荐的品牌信息
        /// </summary>
        /// <returns></returns>
        public static List<BrandrecommendPageInfo> QueryBrandsRecommended(int? pagesize, int? pageindex, ref int numrecord, byte? recommendtype)
        {
            var list = new List<BrandrecommendPageInfo>();
            var data = Factory.ServicesFactory.CreateMallBrandrecommendServices().GetData_AppIndex(pagesize, pageindex, ref numrecord, recommendtype);
            var brandrecommendInfoList = (List<BrandInfo>)data[BrandInfo.TableName]; //推荐的品牌列表  
            var productInfoList = (List<ProductInfo>)data[ProductInfo.TableName]; //产品列表
            foreach (var b in brandrecommendInfoList)
            {
                list.Add(new BrandrecommendPageInfo()
                {
                    BrandrecommendInfo = b,
                    ProductInfos = productInfoList.Where(p => p.BrandID == b.BrandID).ToList()
                });
            }
            return list;
        }
        #endregion

        #region App商城首页相关

        /// <summary>
        /// 获取商城首页相关信息
        /// </summary>
        /// <returns></returns>
        public static ResponseAppMallIndex GetAppMallIndex()
        {

            int totalbrand = 0;

            var productAds = AdsenseDataProxy.GetData_ADSenseType(ADSenseTypes.APP商城首页推荐产品, 6).ToList();

            var products = new List<AppMallIndexProduct>();

            foreach (var item in productAds)
            {
                var product_para = item.Parms;
                int productid = 0;
                decimal sale = 0m;
                decimal jf = 0m;
                if (!string.IsNullOrEmpty(product_para))
                {
                    var _array_ = product_para.Split('|');
                    if (_array_.Length > 0)
                        productid = int.Parse(_array_[0]);
                    if (_array_.Length > 1)
                        sale = decimal.Parse(_array_[1]);
                    if (_array_.Length > 2)
                        jf = decimal.Parse(_array_[2]);
                }
                var product = new AppMallIndexProduct()
                {
                    ProductID = productid,
                    JiFen = jf,
                    Sale = sale,
                    ProductImage = item.PictureFolderName + "/" + item.ImageName,
                    ProductName = item.Caption
                };
                products.Add(product);
            }

            var response = new ResponseAppMallIndex()
            {
                AppTopAds = AdsenseDataProxy.GetData_ADSenseType(ADSenseTypes.APP商城顶部广告, 5).ToList(),
                AppBottomAd = AdsenseDataProxy.GetData_ADSenseType(ADSenseTypes.APP商城底部广告, 1).ToList(),
                AppBrandCommend = Factory.ShopProductFactory.QueryBrandsRecommended(8, 1, ref totalbrand, null),
                AppProducts = products,
                IsSuccess = true
            };
            return response;
        }

        #endregion


        #region 特惠产品

        /// <summary>
        /// 获取特惠产品信息
        /// </summary>
        /// <returns></returns> 
        public static List<PageProductInfo> QueryTehuiProducts(int? pagesize, int? pageindex, ref int numrecord)
        {
            var list = new List<PageProductInfo>();
            //获取特惠产品信息
            var datas = Factory.ServicesFactory.CreateMallProductServices().GetData_Paging(pagesize, pageindex, ref numrecord, null, null, null, null, true, null, null, true, true, null, null, null, null, null, null);
            //产品列表
            var productInfoList = (List<ProductInfo>)datas[ProductInfo.TableName];
            //产品属性列表
            var propertyList = (List<ProductpropertyInfo>)datas[ProductpropertyInfo.TableName];
            //组装特惠产品数据
            foreach (var p in productInfoList)
            {
                list.Add(new PageProductInfo()
                {
                    Product = p,
                    ProductProperty = propertyList.FirstOrDefault(n => n.ProductID == p.ProductID)
                });
            }
            return list;
        }

        #endregion
    }
}
