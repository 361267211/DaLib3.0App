function footer_sys_temp1() {
  var footer_sys_temp1_html = `<div class="footer-temp1-page child_bg cfff">
  <span v-html="footer_sys_temp1_list.content" class="footer-fwb"></span>
</div>`;



var list = document.getElementsByClassName('footer_sys_temp1');
for (var i = 0; i < list.length; i++) {
  if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
    list[i].setAttribute('class', 'footer_sys_temp1 jl_vip_zt_vray');
    new Vue({
      el: '#' + list[i].lastChild.id,
      data() {
        return {
          fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
          post_url_vip: window.apiDomainAndPort,
          footer_sys_temp1_list: {},
          // footer_sys_temp1_list: [
          //   { title: '清华大学图书馆' },
          //   { title: '自动化学报' },
          //   { title: '国家科技图书文献中心' },
          //   { title: '国家图书馆' },
          //   { title: '中科院文献情报中心' },
          //   { title: '中科院邮件系统' },
          // ],
        }
      },
      template: footer_sys_temp1_html,
      mounted() {
        this.footer_sys_temp1_initData();
      },
      methods: {
        footer_sys_temp1_menuClick(url) { //菜单点击
          if (url != this.$route.path) {
            this.$router.push(url)
          }
        },
        footer_sys_temp1_initData() {
          var post_url = this.post_url_vip+'/scenemanage/api/header-footer/footer-data?templatecode=/footer_sys/temp1';
          axios({
            url: post_url,
            method: 'GET',
            headers: {
              'Content-Type': 'text/plain',
            },
          }).then(res => {
            if (res.data && res.data.statusCode == 200) {
              this.footer_sys_temp1_list = res.data.data || {};
            }
          }).catch(err => {
            console.log(err);
          });
        },
        login() {
          alert('登录');
        },
      }
    });
  }
}
}
footer_sys_temp1()