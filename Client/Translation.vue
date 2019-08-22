<template>
  <section class="translation" v-if="this.isTranslation">
    <h4>Перевод</h4>
    <div class="translation">
      <div
        :class="{hidden: !isTranslation, li:isList}"
        v-bind:key="index"
        v-for="(item,index) in this.translationParts"
      >
        {{item}}
        <br />
      </div>
    </div>
  </section>
</template>

<script>
export default {
  props: {
    data: String
  },
  computed: {
    translationParts: function() {
      if (this.data) {
        return this.data.split("\n");
      }
    },
    isTranslation: function() {
      return this.data.match(/[A-Za-z]/);
    },
    isList: function() {
      return this.data.match(/^\s*\d/);
    }
  }
};
</script>

<style lang="scss">
div.translation {
  .hidden {
    display: none;
  }
  .li {
    margin-bottom: 8px;
  }
}
</style>