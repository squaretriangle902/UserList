﻿using Denis.UserList.Common.Entities;
using Denis.UserList.Common.Libraries;
using Denis.UserList.DAL.Fake;
using Denis.UserList.DAL.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Denis.UserList.BLL.Core
{
    public class AwardLogic : IAwardLogic
    {
        private readonly IAwardDAO awardDAO;

        public AwardLogic()
        {
            awardDAO = new AwardDAO();
        }

        public IEnumerable<Award> GetAwardsByUserID(int userID)
        {
            try
            {
                return awardDAO.GetAwardsByUserID(userID);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get awards by user ID", exception);
            }
        }

        public IEnumerable<Award> GetAllAwards()
        {
            try
            {
                return awardDAO.GetAllAwards();
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot get all award", exception);
            }
        }

        public int AddAward(string? name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException();
                }
                return awardDAO.AddAward(name);
            }
            catch (Exception exception)
            {
                throw new BLLException("Cannot add award", exception);
            }
        }
    }
}