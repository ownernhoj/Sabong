﻿using System;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Sabong.Repository.EntityModel;
using System.Security.Cryptography;
using Sabong.Util;

namespace Sabong.Repository.Repo
{
    public class ChickenWin
    {
        public string ChickenResult { get; set; }
    }

    public class UserRepository
    {

        public void UPdatePassWord(user trans)
        {
            try
            {
                using (var context = new s_dbEntities())
                {
                    using (var md5Hash = MD5.Create())
                    {
                        var hashPass = GetMd5Hash(md5Hash, trans.password);
                        trans.password = hashPass;
                        context.users.Attach(trans);

                        var entry = context.Entry(trans);
                        entry.State = EntityState.Modified;

                        //entry.Property(e => e.nn).IsModified = false;


                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }
        //select * from `currency` where `slno`='$currency_id'
        public string GetCurrencyName(int currencyId)
        {
           // if(currencyId)
            try
            {
                using (var context = new s_dbEntities())
                {
                    var result = context.currencies.FirstOrDefault(i => i.slno == currencyId);
                    if (result != null) return result.currency1;
                    return "";
                }
            }
            catch (Exception ex)
            {
                ex.LogError("userId: " + currencyId);
                return string.Empty;
            }
            
        }
        //  $queryplpt=mysql_query("SELECT * FROM `player_pt_calc` where player_id='$_SESSION[useridval]'");
        public player_pt_calc GetPlayerPtByUserId(int userId)
        {
            try
            {
                using (var context = new s_dbEntities())
                {
                    var result = context.player_pt_calc.FirstOrDefault(i => i.player_id == userId);
                    return result;
                }
            }
            catch (Exception ex)
            {
                ex.LogError("userId: " + userId);
                return null;
            }
        }

        //select `currency_type` from `user` where `slno`='$userid'----> get currency
        //select `balance` from `openning_balance` where `date` like '%$date%' and `userid`='$userid'

        //  mysql_query("update `bidpoints` set `updated_bidpoint`=`updated_bidpoint`-$sacceptval where `agent_id`='$_SESSION[useridval]'")or die();

        public void UpdateBidPoint(bidpoint creditBalance)
        {
            try
            {
                using (var context = new s_dbEntities())
                {
                    context.bidpoints.Attach(creditBalance);

                    var entry = context.Entry(creditBalance);
                    entry.State = EntityState.Modified;

                    entry.Property(e => e.bidpoint1).IsModified = false;

                    entry.Property(e => e.agent_id).IsModified = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
            
        }

        public playerbet_limit GetPlayerbetLimit(int userId)
        {
            try
            {
                using (var context = new s_dbEntities())
                {
                    var result = context.playerbet_limit.FirstOrDefault(i => i.playerid == userId);
                    return result;
                }
            }
            catch (Exception ex)
            {
                ex.LogError("userId: " + userId);
                return null;
            }
            
        }
         public double DayCashBalance(int userId,DateTime todayDateTime)
         {
             try
             {
                 using (s_dbEntities context = new s_dbEntities())
                 {
                     var result = context.openning_balance.FirstOrDefault(i => i.userid == userId && i.date == todayDateTime);

                     return result != null ? result.balance : 0;
                 }
             }
             catch (Exception ex)
             {
                 ex.LogError("userId: " + userId);
                 return 0;
             }
         }

         //select `unique_idval` from `user` where `slno`='$userid'
         public string GetUniqueIdVal(int userId)
         {
             try
             {
                 using (var context = new s_dbEntities())
                 {
                     var result = context.users.FirstOrDefault(i => i.slno == userId);

                     return result != null ? result.unique_idval : "";
                 }
             }
             catch (Exception ex)
             {
                 ex.LogError("userId: " + userId);
                 return string.Empty;
             }
         }

         //$q=mysql_query("select sum(winloseamnt) as `totalamnt`,sum(playerbcamt) as totalbetcomm from `transaction` where `playerid`='".$_SESSION['useridval']."'");
         //$trasfer=mysql_query("select coalesce(sum(`amount`),0) as `totaltrans` from `multiple_transfer` where `transfer_to`='$_SESSION[useridval]'  and `type`='transfer'");

         public double GetCashBalance(int userId)
         {
             try
             {
                 using (var context = new s_dbEntities())
                 {

                     var calculateProfitLoss = context.Database.SqlQuery<ProfitLoss>(
                          @"select (select IFNULL(sum(winloseamnt),0) as `totalamnt` from `transaction` where `playerid`={0} ) as totalamnt,

(select coalesce(sum(`amount`),0) as `totaltrans` from `multiple_transfer` where `transfer_to`={0}  and `type`='transfer') as totaltrans", userId).FirstOrDefault();

                     if (calculateProfitLoss == null)
                     {
                         return 0;
                     }


                     return calculateProfitLoss.totalamnt + calculateProfitLoss.totaltrans;
                 }
             }
             catch (Exception ex)
             {
                 ex.LogError("userId: " + userId);
                 return 0;
             }
         }

        public double GetProfit(int userId)
        {
            try
            {
                using (var context = new s_dbEntities())
                {
                    var totalamnt = context.Database.SqlQuery<double>(
                         @"select coalesce(sum(winloseamnt),0) as `totalamnt` from `transaction` where `playerid`={0}", userId).FirstOrDefault();
                    var totalbetcomm =
                        context.Database.SqlQuery<double>(
                            @"select coalesce(sum(comm),0) as `totalbetcomm` from `playerbetcomm` where `playerid`={0} and `type`='betcomm'",
                            userId).FirstOrDefault();
                    var totaltrans =
                        context.Database.SqlQuery<double>(
                            @"select coalesce(sum(`amount`),0) as `totaltrans` from `multiple_transfer` where `transfer_to`={0}  and `type`='transfer'",
                            userId).FirstOrDefault();
                    return totalamnt + totalbetcomm + totaltrans;
                }
            }
            catch (Exception ex)
            {
                ex.LogError("userId: " + userId);
                return 0;
            }
        }

         public string GetCurrencyValueByUserId(int userid)
         {
             try
             {
                 using (var context = new s_dbEntities())
                 {
                     var query = string.Format("select `t1`.`value` AS `value` from (`currency` `t1` join `user` `t2` on((`t1`.`slno` = `t2`.`currency_type`))) where (`t2`.`slno` = {0})", userid);
                     var retval = context.Database.SqlQuery<string>(
                        query).FirstOrDefault();
                     return retval;
                 }
             }
             catch (Exception ex)
             {
                 ex.LogError("userId: " + userid);
                 return string.Empty;
             }
         }
         public double GetBetCredit(int userId)
         {
             try
             {
                 using (s_dbEntities context = new s_dbEntities())
                 {

                     var calculateProfitLoss = context.Database.SqlQuery<ProfitLoss>(
                          @"select (select sum(winloseamnt) as `totalamnt` from `transaction` where `playerid`={0} ) as totalamnt,

(select coalesce(sum(`amount`),0) as `totaltrans` from `multiple_transfer` where `transfer_to`={0}  and `type`='transfer') as totaltrans", userId).FirstOrDefault();

                     if (calculateProfitLoss == null)
                     {
                         return 0;
                     }

                     //SELECT coalesce(sum(`acceptedamount`),0) as `creditbet` FROM `transaction` WHERE `playerid`='$_SESSION[useridval]' and `matchno` in (select `slno` from `fight_assign` where date='$datebe' and winner_cockid=0 and cancelstatus=0)
                     return calculateProfitLoss.totalamnt + calculateProfitLoss.totaltrans;
                 }
             }
             catch (Exception ex)
             {
                 ex.LogError("userId: " + userId);
                 return 0;
             }
         }
        
         //select `bidpoint` from `bidpoints` where `agent_id`='$agntid'
         public double GetCreditBalance(int userId)
         {
             try
             {
                 using (var context = new s_dbEntities())
                 {
                     var result = context.bidpoints.FirstOrDefault(i => i.agent_id == userId);

                     return result != null ? result.bidpoint1 : 0;
                 }
             }
             catch (Exception ex)
             {
                 ex.LogError("userId: " + userId);
                 return 0;
             }
         }
         public double GetUpdatedCreditBalance(int userId)
         {
             try
             {
                 using (s_dbEntities context = new s_dbEntities())
                 {
                     var result = context.bidpoints.FirstOrDefault(i => i.agent_id == userId);

                     return result != null ? result.updated_bidpoint : 0;
                 }
             }
             catch (Exception ex)
             {
                 ex.LogError("userId: "+userId);
                 return 0;
             }
         }
        public user Login(string username, string password)
        {
            try
            {
                using (var context = new s_dbEntities())
                {
                    using (var md5Hash = MD5.Create())
                    {
                        string hashPass = GetMd5Hash(md5Hash, password);

                        return context.users.FirstOrDefault(i => i.username == username && i.password == hashPass);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogError(string.Format("username: {0},password: {1}",username,password));
                return null;
            }
            
        }

        public user GetUser(int id)
        {
            try
            {
                using (var context = new s_dbEntities())
                {
                    return context.users.FirstOrDefault(i => i.slno == id);
                }
            }
            catch (Exception ex)
            {
                ex.LogError(string.Format("UserId: {0}",id));
                return null;
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
         {

             // Convert the input string to a byte array and compute the hash. 
             var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

             // Create a new Stringbuilder to collect the bytes 
             // and create a string.
             var sBuilder = new StringBuilder();

             // Loop through each byte of the hashed data  
             // and format each one as a hexadecimal string. 
             for (var i = 0; i < data.Length; i++)
             {
                 sBuilder.Append(data[i].ToString("x2"));
             }

             // Return the hexadecimal string. 
             return sBuilder.ToString();
         }

        public bidpoint getBidPoint(int playerid)
        {
            try
            {
                using (s_dbEntities context = new s_dbEntities())
                {
                    return context.bidpoints.FirstOrDefault(i => i.agent_id == playerid);
                }
            }
            catch (Exception ex)
            {
                ex.LogError(string.Format("playerid: {0}",playerid));
                return null;
            } 
        }
    }

    public class ProfitLoss
    {
        public double totaltrans { get; set; }

        public double totalamnt { get; set; }
    }

    public class AllLevelComm
    {
        public float AgentBetComm { get; set; }
        public float MasterBetComm { get; set; }
        public float SrmasterBetComm { get; set; }
        public float HouseBetComm { get; set; }
        public float AdminBetComm { get; set; }
    }
}
