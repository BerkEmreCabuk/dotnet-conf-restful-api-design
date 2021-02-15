using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Infrastructures.Mapper
{
    interface IMapping
    {
        void CreateMappings(IProfileExpression profileExpression);
    }
}
