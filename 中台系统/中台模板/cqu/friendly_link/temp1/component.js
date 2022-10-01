function cqu_friendly_link_temp1() {
  var template_html = `<div class="index-footer">
  <div class="index-footer-box">
    <div class="index-footer-link" v-if="cqu_list&&cqu_list.assemblyLink&&cqu_list.assemblyLink.length>0">
      <h4>专题网站</h4>
      <div>
        <span v-for="(item,index) in cqu_list.assemblyLink" :key="index" @click="cqu_linkTo(item.outUrl)">{{item.title}}</span>
      </div>
    </div>
    <div class="index-footer-link" v-if="cqu_list&&cqu_list.fastLink&&cqu_list.fastLink.length>0">
      <h4>快速链接</h4>
      <div>
        <span v-for="(item,index) in cqu_list.fastLink" :key="index" @click="cqu_linkTo(item.outUrl)">{{item.title}}</span>
      </div>
    </div>
    <div class="index-footer-img">
      <div class="index-footer-img-box" @click="cqu_linkTo(cqu_list.allianceCertifyUrl)">
        <div><img :src="imgPath+'cqu/icon-zhitulianmeng.png'" alt=""></div>
        <span>智图联盟成员</span>
      </div>
      <div class="index-footer-img-box1" @click="cqu_bigImg('wx')">
        <div><img :src="imgPath+'cqu/cd_wx.jpg'" alt=""></div>
        <span>微信关注</span>
        <span class="footer-img-box1-cur">点击放大</span>
      </div>
      <div class="index-footer-img-box1" @click="cqu_bigImg('wb')">
        <div><img :src="imgPath+'cqu/cd_wb.png'" alt=""></div>
        <span>微博关注</span>
        <span class="footer-img-box1-cur">点击放大</span>
      </div>
      <div class="index-footer-img-box1" @click="cqu_bigImg('cx')">
        <div><img :src="imgPath+'cqu/cd_cx.jpg'" alt=""></div>
        <span>移动图书馆</span>
        <span class="footer-img-box1-cur">点击放大</span>
      </div>
    </div>
  </div>
  <div class="index-qr-box" v-show="cqu_big_show" @click="cqu_big_show=false">
    <img :src="cqu_big_qr" alt="" @click.stop="">
  </div>
</div>`;

  var list = document.getElementsByClassName('cqu_friendly_link_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_friendly_link_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            imgPath: window.localStorage.getItem('fileUrl')+'/public/image/',//公共图片路径
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            cqu_list: null,
            cqu_big_show: false,
            cqu_big_qr: '',
            cqu_big_qr_data: {},
            // cqu_big_qr_data: {
            //   wx: require('../images/cd_wx.jpg'),
            //   wb: require('../images/cd_wb.png'),
            //   cx: require('../images/cd_cx.jpg'),
            // },
          }
        },
        created () {
          this.cqu_big_qr_data = {
            wx: this.imgPath + 'cqu/cd_wx.jpg',
            wb: this.imgPath + 'cqu/cd_wb.png',
            cx: this.imgPath + 'cqu/cd_cx.jpg',
          };
        },
        mounted() {
          var template_temp_data_list = [];
          if (template_temp_set_list) {
            for (var i = 0; i < template_temp_set_list.length; i++) {
              var topCount = template_temp_set_list[i].topCount;
              var columnid = template_temp_set_list[i].id;
              var OrderRule = template_temp_set_list[i].sortType;
              template_temp_data_list.push({ count: topCount, columnId: columnid, sortField: OrderRule });
            }
          }
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_bigImg(img) {
            this.cqu_big_show = true;
            this.cqu_big_qr = this.cqu_big_qr_data[img];
          },
          cqu_linkTo(url) {
            window.open(url);
          },
          cqu_initData() {
            var post_url = this.post_url_vip + '/navigation/api/navigation/link-list';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_list = res.data.data;
              }
            }).catch(err => {
              console.log(err);
            });
          },
        },
      });
    }
  }
}
cqu_friendly_link_temp1()