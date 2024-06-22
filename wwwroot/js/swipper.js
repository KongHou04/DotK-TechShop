var swiper = new Swiper(".hotDealSwiper", {
  breakpoints: {
    0: {
      slidesPerView: 2,
    },
    768: {
      slidesPerView: 3,
    },
    992: {
      slidesPerView: 3,
    },
    1200: {
      slidesPerView: 4,
    },
  },
  spaceBetween: 30,
  freeMode: true,
  pagination: {
    el: ".hotDeal-swiper-pagination",
    clickable: true,
  },
});

var swiper = new Swiper(".footerSwiper", {
  slidesPerView: 1,
  freeMode: true,
  pagination: {
    el: ".footer-swiper-pagination",
    clickable: true,
  },
});
