﻿using AutoMapper;
using OoadProject.Core.AppSession;
using OoadProject.Data.Entity.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoadProject.Core.Services
{
    public class BaseService
    {
        protected IMapper _mapper;
        protected IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = AutoMapper.Config.CreateMapper();
                }
                return _mapper;
            }
        }

        protected User _currentUser;
        protected User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = Session.CurrentUser;
                }
                return _currentUser;
            }
        }
    }
}
