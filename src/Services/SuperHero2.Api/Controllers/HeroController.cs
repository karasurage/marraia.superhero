﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marraia.Notifications.Base;
using Marraia.Notifications.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperHero.Application.AppHero.Input;
using SuperHero.Application.AppHero.Interfaces;

namespace SuperHero2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : BaseController
    {
        private readonly IHeroAppService _heroAppService;
        public HeroController(INotificationHandler<DomainNotification> notification, 
            IHeroAppService heroAppService)
            : base (notification)
        {
            _heroAppService = heroAppService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] HeroInput input)
        {
            var item = await _heroAppService
                                .Insert(input)
                                .ConfigureAwait(false);

            return CreatedContent("", item);
        }

        [HttpGet] //api/hero
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            return OkOrNoContent(_heroAppService.Get());
        }

        [HttpGet] //api/hero/id
        [Route("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return OkOrNotFound(await _heroAppService
                                        .GetByIdAsync(id)
                                        .ConfigureAwait(false));
        }
    }
}
