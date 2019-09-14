import Vue from "vue";
import VueRouter from "vue-router";
import App from "./App.vue";
import { library } from "@fortawesome/fontawesome-svg-core";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

library.add(faSearch);

Vue.component("font-awesome-icon", FontAwesomeIcon);

Vue.config.productionTip = false;
Vue.use(VueRouter);

const routes = [{ path: "/:lemma", component: App }];

new Vue({
  router: new VueRouter({ routes }),
  render: h => h(App)
}).$mount("#app");
